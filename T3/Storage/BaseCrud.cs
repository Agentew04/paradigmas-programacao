using MySqlConnector;

namespace BolsaValores.Storage; 


/// <summary>
/// Uma classe base para todos os controlados de dados para
/// uma conexão ao MySQL.
/// </summary>
/// <typeparam name="T">O tipo que será serializado</typeparam>
public class BaseCrud<T> : ICrud<T> {

    private readonly MySqlConnection _conn;

    public BaseCrud() {
        _conn = DatabaseConnector.GetConn();
    }

    public void Add(T t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        SqlFactory.GetInsertSql(typeof(T));
        SqlFactory.FillCommand(cmd, t);
        cmd.ExecuteNonQuery();
    }

    public void Add(IEnumerable<T> t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        foreach (T item in t) {
            SqlFactory.FillCommand(cmd,item);
            cmd.ExecuteNonQuery();
        }
    }

    public IEnumerable<T> Select() {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        List<T> results = new();
        cmd.CommandText = SqlFactory.GetSelectSql(typeof(T));
        SqlFactory.FillCommand(cmd, default(T));
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
            results.Add(SqlFactory.ReadItem<T>(reader));
        return results;
    }

    public IEnumerable<T> Select(Func<T,bool> predicate) {
        return Select().Where(predicate);
    }

    public int Update(Func<T, bool> predicate, T t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        SqlFactory.FillCommand(cmd,t);
        return cmd.ExecuteNonQuery();
    }
    
    public int Update(Func<T, bool> predicate, Func<T,T> updater) {
        IEnumerable<T> items = Select(predicate);
        var count = 0;
        foreach (T item in items) {
            T newItem = updater(item);
            Update(x => x.Equals(item), newItem);
            count++;
        }

        return count;
    }
    
    public int Remove(T t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        SqlFactory.FillCommand(cmd,t);
        return cmd.ExecuteNonQuery();
    }

    public int Remove(Func<T, bool> predicate) {
        IEnumerable<T> items = Select(predicate);
        return items.Sum(Remove);
    }
}