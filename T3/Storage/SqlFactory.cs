using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
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

        public FieldInfo? FieldInfo { get; init; }
        public PropertyInfo? PropertyInfo { get; init; }
    }

    private static IEnumerable<Member> GetMembers(Type type) {
        List<Member> members = type.GetFields()
            .Select(field => new Member {
                Name = field.Name, 
                PrimaryKey = field.GetCustomAttribute<PrimaryKeyAttribute>() is not null, 
                Column = field.GetCustomAttribute<ColumnAttribute>()?.Name ?? field.Name,
                FieldInfo = field,
                PropertyInfo = null
            }).ToList();
        
        members.AddRange(type.GetProperties()
            .Select(property => new Member {
                Name = property.Name, 
                PrimaryKey = property.GetCustomAttribute<PrimaryKeyAttribute>() is not null, 
                Column = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name,
                PropertyInfo = property,
                FieldInfo = null
            }));
        return members;
    }

    private static bool IsSerializable(MemberInfo type) {
        SerializableAttribute? serializable = type.GetCustomAttribute<SerializableAttribute>();
        return serializable is not null;
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
        foreach (Member member in members) {
            columns.Append(member.Column + ',');
            values.Append('@' + member.Column + ',');
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

    public static void FillCommand<T>(MySqlCommand cmd, T t) {
        Type type = typeof(T);

        IEnumerable<string> parameters = GetParameters(cmd.CommandText);
        foreach (string parameter in parameters) {
            Member paramMember = GetMembers(type)
                .FirstOrDefault(x => x.Column == parameter);

            object? value = paramMember.PropertyInfo?.GetValue(t) ?? paramMember.FieldInfo?.GetValue(t);
            cmd.Parameters.AddWithValue('@' + parameter, value);
        }
    }

    public static T? ReadItem<T>(MySqlDataReader reader) {
        if (!reader.HasRows)
            return default;

        Type type = typeof(T);
        IEnumerable<Member> members = GetMembers(type);

        T t = Activator.CreateInstance<T>();

        for (var i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);
            object columnValue = reader.GetValue(i);

            Member member = members.FirstOrDefault(x => x.Column == columnName);

            bool isField = member.FieldInfo is not null && member.PropertyInfo is null;
            if (isField) {
                member.FieldInfo?.SetValue(t, columnValue);
            }else {
                member.PropertyInfo?.SetValue(t, columnValue);
            }
        }


        return t;
    }
}