using MySqlConnector;

namespace BolsaValores.Storage; 


/// <summary>
/// Uma classe base para todos os controlados de dados para
/// uma conexão ao MySQL.
/// </summary>
/// <typeparam name="T">O tipo que será serializado</typeparam>
public class Crud<T> : ICrud<T> {

    private readonly MySqlConnection _conn;

    public Crud() {
        _conn = DatabaseConnector.GetConn();
    }

    public void Add(T t) {
        try {
            _conn.Open();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = SqlFactory.GetInsertSql(typeof(T));
            SqlFactory.FillCommand(cmd, t);
            cmd.ExecuteNonQuery();
        }
        finally {
            _conn.Close();
        }
    }

    public void Add(IEnumerable<T> t) {
        try {
            _conn.Open();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = SqlFactory.GetInsertSql(typeof(T));
            foreach (T item in t) {
                SqlFactory.FillCommand(cmd, item);
                cmd.ExecuteNonQuery();
            }
        }
        finally {
            _conn.Close();
        }
    }

    public IEnumerable<T> Select() {
        try {
            _conn.Open();
            List<T> results = new();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = SqlFactory.GetSelectSql(typeof(T));
            SqlFactory.FillCommand(cmd, default(T));
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                T? item = SqlFactory.ReadItem<T>(reader);
                if (item is not null)
                    results.Add(item);
            }

            return results;
        }
        finally {
            _conn.Close();
        }
    }

    public IEnumerable<T> Select(Func<T,bool> predicate) {
        return Select().Where(predicate);
    }

    public int Update(Func<T, bool> predicate, T t) {
        try {
            _conn.Open();
            MySqlCommand cmd = _conn.CreateCommand();
            SqlFactory.FillCommand(cmd, t);
            return cmd.ExecuteNonQuery();
        }
        finally {
            _conn.Close();
        }
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
        try {
            _conn.Open();
            MySqlCommand cmd = _conn.CreateCommand();
            SqlFactory.FillCommand(cmd, t);
            return cmd.ExecuteNonQuery();
        }
        finally {
            _conn.Close();
        }
    }

    public int Remove(Func<T, bool> predicate) {
        IEnumerable<T> items = Select(predicate);
        return items.Sum(Remove);
    }
}