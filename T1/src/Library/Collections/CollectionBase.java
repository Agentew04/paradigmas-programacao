package Library.Collections;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.function.Predicate;
import java.util.stream.Stream;

/**
 * Implements a basic CRUD interface for a collection of items.
 * @param <T> The type of the items in the collection
 */
public abstract class CollectionBase<T> implements Iterable<T> {
    protected final Connection conn;

    /**
     * Creates a new CollectionBase instance.
     * @param conn The connection to the database
     */
    public CollectionBase(Connection conn) {
        this.conn = conn;
    }

    /**
     * Returns a list of all items in the collection.
     * @return The list of items
     */
    public List<T> get() {
        ArrayList<T> list = new ArrayList<>();
        try{
            PreparedStatement stmt = getSelectSQL(conn);
            ResultSet rs = stmt.executeQuery();
            while(rs.next()) {
                list.add(readResult(rs));
            }
            rs.close();
        }catch(Exception e) {
            System.out.println(e.getMessage());
        }
        return list;
    }

    /**
     * Inserts the given item into the database.
     * @param item The item to insert
     */
    public void insert(T item) {
        try{
            PreparedStatement stmt = getInsertSQL(conn, item);
            stmt.executeUpdate();
        }catch(Exception e) {
            System.out.println(e.getMessage());
        }
    }

    /**
     * Deletes the given item from the database.
     * Only compares items using the primary key.
     * @param item The item to delete
     */
    public void delete(T item) {
        try{
            PreparedStatement stmt = getDeleteSQL(conn, item);
            stmt.executeUpdate();
        }catch(Exception e) {
            System.out.println(e.getMessage());
        }
    }

    /**
     * Deletes all items that match the given predicate.
     * @param predicate The predicate to test the items with
     */
    public int delete(Predicate<T> predicate){
        int deleted = 0;
        for(T item : get()){
            if(predicate.test(item)){
                delete(item);
                deleted++;
            }
        }
        return deleted;
    }

    /**
     * Updates the given item in the database.
     * The primary key is used to find the item to update.
     * @param item The item to update
     */
    public void update(T item){
        try{
            PreparedStatement stmt = getUpdateSQL(conn, item);
            stmt.executeUpdate();
        }catch(Exception e) {
            System.out.println(e.getMessage());
        }
    }

    /**
     * Returns the first item that matches the given predicate.
     * @param predicate The predicate to test the items with
     * @return The first item that matches the predicate or null if no item matches
     */
    public T getOne(Predicate<T> predicate){
        for(T item : get()){
            if(predicate.test(item)){
                return item;
            }
        }
        return null;
    }

    /**
     * Returns a list of all items that match the given predicate.
     * @param predicate The predicate to test the items with
     * @return The list of items that match the predicate
     */
    public List<T> get(Predicate<T> predicate){
        ArrayList<T> list = new ArrayList<>();
        for(T item : get()){
            if(predicate.test(item)){
                list.add(item);
            }
        }
        return list;
    }

    /**
     * Returns true if any item matches the given predicate.
     * @param predicate The condition to test the items with
     * @return True if any item matches the predicate
     */
    public boolean has(Predicate<T> predicate){
        for(T item : get()){
            if(predicate.test(item)){
                return true;
            }
        }
        return false;
    }

    /**
     * Returns a new instance of the item that is read from the ResultSet.
     * @param rs The ResultSet to read from
     * @return The new item
     */
    public abstract T readResult(ResultSet rs) throws SQLException;

    /**
     * Returns a PreparedStatement that selects all items from the database.
     * @param conn The connection that creates the statement
     * @return The Statement already filled with the item's data
     * @throws SQLException Thrown if an error occurs
     */
    protected abstract PreparedStatement getSelectSQL(Connection conn) throws SQLException;

    /**
     * Returns a PreparedStatement that inserts the given item into the database.
     * @param conn The connection that creates the statement
     * @param item The item to insert
     * @return The Statement already filled with the item's data
     */
    protected abstract PreparedStatement getInsertSQL(Connection conn, T item) throws SQLException;


    /**
     * Returns a PreparedStatement that deletes the given item from the database.
     * Should only compare items using the respective primary key/id.
     * @param conn The connection
     * @param item The item to delete
     * @return The Statement already filled with the item's data
     */
    protected abstract PreparedStatement getDeleteSQL(Connection conn, T item) throws SQLException;

    /**
     * Returns a PreparedStatement that updates the given item in the database.
     * Should only compare items using the respective primary key/id.
     * @param conn The connection that creates the statement
     * @param item The item that should be updated with its new data
     * @return The Statement already filled with the item's data
     */
    protected abstract PreparedStatement getUpdateSQL(Connection conn, T item) throws SQLException;

    public Stream<T> stream() {
        return get().stream();
    }

    public Iterator<T> iterator() {
        return get().iterator();
    }
}
