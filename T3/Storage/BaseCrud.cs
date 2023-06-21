using MySqlConnector;

namespace BolsaValores.Storage; 


/// <summary>
/// Uma classe base para todos os controlados de dados para
/// uma conexão ao MySQL.
/// </summary>
/// <typeparam name="T">O tipo que será serializado</typeparam>
public abstract class BaseCrud<T> : ICrud<T> {

    private readonly MySqlConnection _conn;

    protected BaseCrud() {
        _conn = DatabaseConnector.GetConn();
    }

    
    protected abstract void AddCommand(MySqlCommand cmd, T t);


    public void Add(T t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        AddCommand(cmd, t);
        cmd.ExecuteNonQuery();
    }

    
    public void Add(IEnumerable<T> t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        foreach (T item in t) {
            AddCommand(cmd, item);
            cmd.ExecuteNonQuery();
        }
    }

    
    protected abstract void SelectCommand(MySqlCommand cmd);

    
    public IEnumerable<T> Select() {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        List<T> results = new();
        SelectCommand(cmd);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.NextResult()) 
            results.Add(ReadItem(reader));
        return results;
    }

    
    public IEnumerable<T> Select(Func<T,bool> predicate) {
        return Select().Where(predicate);
    }


    protected abstract void UpdateCommand(MySqlCommand cmd, T t);
    
    
    public int Update(Func<T, bool> predicate, T t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        UpdateCommand(cmd, t);
        return cmd.ExecuteNonQuery();
    }

    
    public int Update(Func<T, bool> predicate, Func<T,T> updater) {
        IEnumerable<T> items = Select(predicate);
        int count = 0;
        foreach (T item in items) {
            T newItem = updater(item);
            Update(x => x.Equals(item), newItem);
            count++;
        }

        return count;
    }


    protected abstract string RemoveCommand(MySqlCommand cmd, T t);

    
    public int Remove(T t) {
        _conn.Open();
        MySqlCommand cmd = _conn.CreateCommand();
        RemoveCommand(cmd, t);
        return cmd.ExecuteNonQuery();
    }

    
    public int Remove(Func<T, bool> predicate) {
        IEnumerable<T> items = Select(predicate);
        return items.Sum(Remove);
    }

    protected abstract T ReadItem(MySqlDataReader reader);
}