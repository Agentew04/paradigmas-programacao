package Library.Collections;

import Library.Models.Book;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class BookCollection extends CollectionBase<Book> {
    /**
     * Creates a new CollectionBase instance.
     *
     * @param conn The connection to the database
     */
    public BookCollection(Connection conn) {
        super(conn);
    }

    @Override
    public Book readResult(ResultSet rs) throws SQLException {
        return new Book(
                rs.getString("title"),
                rs.getString("author"),
                rs.getString("publisher"),
                rs.getString("isbn"),
                rs.getInt("edition"),
                rs.getInt("year"),
                rs.getInt("id"));
    }

    @Override
    protected PreparedStatement getSelectSQL(Connection conn) throws SQLException {
        return conn.prepareStatement("SELECT * FROM library.books");
    }

    /**
     * Returns an unique id for a new item.
     * @return The new id or -1 if an error occurred
     */
    public int getNewId() {
        try {
            PreparedStatement stmt = getNewIdSQL(conn);
            ResultSet rs = stmt.executeQuery();
            if(rs.next()) {
                return rs.getInt("max") + 1;
            }
        }catch(Exception e) {
            System.out.println(e.getMessage());
        }
        return -1;
    }

    @Override
    protected PreparedStatement getInsertSQL(Connection conn, Book item) throws SQLException {
        item.id = getNewId();
        PreparedStatement stmt = conn.prepareStatement("INSERT INTO library.books (id, title, author, publisher, isbn, edition, year) VALUES (?, ?, ?, ?, ?, ?, ?)");
        stmt.setInt(1, item.id);
        stmt.setString(2, item.title);
        stmt.setString(3, item.author);
        stmt.setString(4, item.publisher);
        stmt.setString(5, item.isbn);
        stmt.setInt(6, item.edition);
        stmt.setInt(7, item.year);
        return stmt;
    }

    @Override
    protected PreparedStatement getDeleteSQL(Connection conn, Book item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("DELETE FROM library.books WHERE id = ?");
        stmt.setInt(1, item.id);
        return stmt;
    }

    @Override
    protected PreparedStatement getUpdateSQL(Connection conn, Book item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("UPDATE library.books SET title = ?, author = ?, publisher = ?, isbn = ?, edition = ?, year = ? WHERE id = ?");
        stmt.setString(1, item.title);
        stmt.setString(2, item.author);
        stmt.setString(3, item.publisher);
        stmt.setString(4, item.isbn);
        stmt.setInt(5, item.edition);
        stmt.setInt(6, item.year);
        stmt.setInt(7, item.id);
        return stmt;
    }

    /**
     * Returns a PreparedStatement that returns the next id for a new item.
     * @param conn The connection that creates the statement
     * @return The Statement already filled with the item's data
     * @throws SQLException Thown if an error occurs
     */
    protected PreparedStatement getNewIdSQL(Connection conn) throws SQLException {
        return conn.prepareStatement("SELECT MAX(id) as max FROM library.books");
    }
}
