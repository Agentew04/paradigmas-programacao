using MySqlConnector;

namespace BolsaValores.Storage; 

/// <summary>
/// Gerencia e cria conexões para o banco de dados
/// </summary>
public static class DatabaseConnector {
    private const string User = "root";
    private const string Pwd = "abc12345";
    private const string Server = "localhost";
    private const string Database = "bolsa";

    private static string GetUrl() {
        return $"server={Server};user={User};database={Database};password={Pwd};";
    }
    
    private static  MySqlConnection? _conn;

    /// <summary>
    /// Retorna a conexão <i>singleton</i> para o banco de dados
    /// configurado.
    /// </summary>
    /// <returns>A conexão</returns>
    public static MySqlConnection GetConn() {
        if (_conn is not null) {
            return _conn;
        }

        _conn = new MySqlConnection(GetUrl());

        return _conn;
    }
}