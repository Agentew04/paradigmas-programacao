using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using BolsaValores.Exceptions;
using BolsaValores.Storage.Attributes;
using MySqlConnector;

namespace BolsaValores.Storage; 

/// <summary>
/// Classe que usa Reflexão e Atributos para gerar sentenças SQL
/// para manipulacao de dados no DB. Também responsável por
/// preencher parâmetros sql.
///
/// Utiliza primariamente os seguintes atributos para extrair metadados:
/// <ul>
///     <li><see cref="SerializableAttribute"/></li>
///     <li><see cref="TableAttribute"/></li>
///     <li><see cref="ColumnAttribute"/></li>
///     <li><see cref="PrimaryKeyAttribute"/></li>
/// </ul>
/// </summary>
public static class SqlFactory {

    private struct Member {

        public string Name { get; init; }
        public string Column { get; init; }
        public bool PrimaryKey { get; init; }
        
        public FieldInfo FieldInfo { get; init; }
    }

    private static IEnumerable<Member> GetMembers(Type type) {
        return type.GetFields()
            .Select(field => new Member {
                Name = field.Name, 
                PrimaryKey = IsPrimaryKey(field), 
                Column = field.GetCustomAttribute<ColumnAttribute>()?.Name ?? field.Name,
                FieldInfo = field
            }).ToList();
    }

    private static bool IsSerializable(MemberInfo info) {
        SerializableAttribute? serializable = info.GetCustomAttribute<SerializableAttribute>();
        return serializable is not null;
    }

    private static bool IsPrimaryKey(MemberInfo info) {
        PrimaryKeyAttribute? primaryKey = info.GetCustomAttribute<PrimaryKeyAttribute>();
        return primaryKey is not null;
    }
    
    /// <summary>
    /// Gera uma string SQL para retornar todos os elementos de um tipo
    /// </summary>
    /// <param name="type">O tipo a ser selecionado</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetSelectSql(Type type) {
        if (!IsSerializable(type)) {
            throw new MissingAttributeException($"The type {type.FullName} is not serializable");
        }
        TableAttribute? tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr is null) {
            throw new MissingAttributeException("A table name was not specified. Declare one with TableAttribute");
        }
        string table = tableAttr.Name;
        
        StringBuilder columns = new();
        IEnumerable<Member> members = GetMembers(type);
        foreach (Member member in members) {
            columns.Append(member.Column+',');
        }

        columns.Remove(columns.Length-1, 1);
        return $"SELECT {columns} FROM {table};";
    }

    /// <summary>
    /// Gera um SQL para inserir <paramref name="type"/> no banco de dados
    /// </summary>
    /// <param name="type"></param>
    /// <returns>O SQL gerado</returns>
    /// <exception cref="Exception"></exception>
    public static string GetInsertSql(Type type) {
        if (!IsSerializable(type)) {
            throw new MissingAttributeException($"The type {type.FullName} is not serializable");
        }
        
        TableAttribute? tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr is null) {
            throw new MissingAttributeException("A table name was not specified. Declare one with TableAttribute");
        }
        string table = tableAttr.Name;

        StringBuilder columns = new();
        StringBuilder values = new();

        IEnumerable<Member> members = GetMembers(type);
        foreach (string column in members.Select(x => x.Column)) {
            columns.Append(column + ',');
            values.Append('@' + column + ',');
        }
        columns.Remove(columns.Length - 1, 1);
        values.Remove(values.Length - 1, 1);
        
        return $"INSERT INTO {table} ({columns}) VALUES ({values});";
    }

    /// <summary>
    /// Gera um SQL para atualizar um tipo baseado em sua <see cref="PrimaryKeyAttribute"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetUpdateSql(Type type) {
        if (!IsSerializable(type)) {
            throw new MissingAttributeException($"The type {type.FullName} is not serializable");
        }
        
        TableAttribute? tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr is null) {
            throw new MissingAttributeException("A table name was not specified. Declare one with TableAttribute");
        }
        string table = tableAttr.Name;

        StringBuilder set = new();

        var primaryName = "";

        IEnumerable<Member> members = GetMembers(type);
        foreach (Member member in members) {
            set.Append($"{member.Column} = @{member.Column},");

            if (member.PrimaryKey && primaryName == "") {
                primaryName = member.Column;
            }
        }
        set.Remove(set.Length - 1, 1);

        var condition = $"{primaryName} = @{primaryName}";
        
        return $"UPDATE {table} SET {set} WHERE {condition};";
    }

    /// <summary>
    /// Gera um SQL para deletar um item do banco de dados.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetDeleteSql(Type type) {
        if (!IsSerializable(type)) {
            throw new MissingAttributeException($"The type {type.FullName} is not serializable");
        }
        
        TableAttribute? tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr is null) {
            throw new MissingAttributeException("A table name was not specified. Declare one with TableAttribute");
        }

        string table = tableAttr.Name;

        var primaryName = "";

        IEnumerable<Member> members = GetMembers(type);
        foreach (Member member in members) {
            if (member.PrimaryKey && primaryName == "") {
                primaryName = member.Column;
            }
        }
        var condition = $"{primaryName} = @{primaryName}";
        
        return $"DELETE FROM {table} WHERE {condition};";
    }

    private static IEnumerable<string> GetParameters(string sql) {
        List<string> parameters = new();
        StringBuilder sb = new();
        var reading = false;
        for (int i = 0; i < sql.Length; i++) {
            if (sql[i] == '@') {
                reading = true;
                continue;
            }

            bool isLower = sql[i] >= 'a' && sql[i] <= 'z';
            bool isUpper = sql[i] >= 'A' && sql[i] <= 'Z';
            if (!isLower && !isUpper && reading) {
                reading = false;
                parameters.Add(sb.ToString());
                sb.Clear();
                continue;
            }
            
            if (reading) {
                sb.Append(sql[i]);
            }
        }

        return parameters;
    }

    /// <summary>
    /// Preenche um comando sql com variáveis de <paramref name="t"/>.
    /// </summary>
    /// <param name="cmd">O comando a ser preenchido</param>
    /// <param name="t">A variável de tipo <typeparamref name="T"/> que fornecerá os dados</param>
    /// <typeparam name="T">O tipo do dado a ser usado</typeparam>
    public static void FillCommand<T>(MySqlCommand cmd, T t) {
        Type type = typeof(T);

        IEnumerable<string> parameters = GetParameters(cmd.CommandText);
        foreach (string parameter in parameters) {
            Member paramMember = GetMembers(type)
                .FirstOrDefault(x => x.Column == parameter);

            object? value = paramMember.FieldInfo?.GetValue(t);
            cmd.Parameters.AddWithValue('@' + parameter, value);
        }
    }

    private static void SetProperty<T>(FieldInfo field, T t, object? value) {
        bool jsonSerialize = field.GetCustomAttribute<JsonSerializeAttribute>() is not null;
        if (jsonSerialize) {
            if(value is not string json)
                throw new ArgumentException($"Argument {nameof(value)} must be a string");
            Type fieldType = field.FieldType;
            object? obj = JsonSerializer.Deserialize(json,fieldType);
            field.SetValue(t, obj);
            return;
        }
        
        // if target field is char and value is string, get first char
        if (field.FieldType == typeof(char) && value is string str) {
            value = str[0];
        }
        
        
        field.SetValue(t, value);
    }

    /// <summary>
    /// Lê um item a partir de um leitor de consultas MySql. Utiliza
    /// reflexão e atributos para descobrir quais campos devem ser
    /// preenchidos.
    /// </summary>
    /// <param name="reader">O leitor da consulta realizada.</param>
    /// <typeparam name="T">O tipo a ser avaliado</typeparam>
    /// <returns>Uma instância de tipo <typeparamref name="T"/> preenchida com os dados lidos
    /// ou <see langword="null"/> se não foi possível ler o objeto.</returns>
    public static T? ReadItem<T>(MySqlDataReader reader) {
        if (!reader.HasRows)
            return default;

        Type type = typeof(T);
        List<Member> members = GetMembers(type).ToList();

        T t = Activator.CreateInstance<T>();

        for (var i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);
            object columnValue = reader.GetValue(i);

            Member member = members.FirstOrDefault(x => x.Column == columnName);
            SetProperty(member.FieldInfo, t, columnValue);
        }

        return t;
    }

    /// <summary>
    /// Cria um texto de consulta SQL para ver qual é o maior valor de uma determinada coluna.
    /// Normalmente usado em colunas que são chaves primárias e são auto incrementadas.
    /// </summary>
    /// <param name="type">O tipo a ser analisado</param>
    /// <param name="columnName">O nome da coluna</param>
    /// <returns>O SQL gerado</returns>
    /// <exception cref="MissingAttributeException">O tipo não tem o atributo <see cref="TableAttribute"/></exception>
    public static string GetMaxSql(Type type, string columnName) {
        TableAttribute? tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr is null) {
            throw new MissingAttributeException("A table name was not specified. Declare one with TableAttribute");
        }
        string table = tableAttr.Name;
        
        string sql = "SELECT MAX(@column) FROM @table;";
        sql = sql.Replace("@column", columnName);
        sql = sql.Replace("@table", table);
        return sql;
    }
}