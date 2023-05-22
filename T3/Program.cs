using MySqlConnector;

namespace BolsaValores;

public static class Program {
    public static void Main(string[] args) {
        Menu menu = new();

        menu.StartMenu();
        
        
        string user = "root";
        string pwd = "abc12345";
        string server = "localhost";
        string database = "bolsa";
        string url = $"server={server};user={user};database={database};password={pwd};";
        MySqlConnection conn = new MySqlConnection(url);
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM ativos";
        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            Console.WriteLine(reader.GetString("ticker"));
        }
    }
}