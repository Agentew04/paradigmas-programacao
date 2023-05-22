package Library.Collections;

import Library.Models.Reservation;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class ReservationCollection extends CollectionBase<Reservation>{
    /**
     * Creates a new ReservationCollection instance.
     *
     * @param conn The connection to the database
     */
    public ReservationCollection(Connection conn) {
        super(conn);
    }

    @Override
    public Reservation readResult(ResultSet rs) throws SQLException {
        BookCollection bookReader = new BookCollection(null);
        UserCollection userReader = new UserCollection(null);
        return new Reservation(
                bookReader.readResult(rs),
                userReader.readResult(rs)
        );
    }

    @Override
    protected PreparedStatement getSelectSQL(Connection conn) throws SQLException {
        return conn.prepareStatement("""
            select \s
            books.title, books.id, books.author, books.publisher, books.isbn, books.edition, books.year,
            users.login, users.name, users.teacher
            from reservations
            inner join books on book_id = books.id
            inner join users on user_login = users.login;""");
    }

    @Override
    protected PreparedStatement getInsertSQL(Connection conn, Reservation item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("INSERT INTO library.reservations (book_id, user_login) VALUES (?, ?);");
        stmt.setInt(1, item.book.id);
        stmt.setString(2, item.user.login);
        return stmt;
    }

    @Override
    protected PreparedStatement getDeleteSQL(Connection conn, Reservation item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("DELETE FROM library.reservations WHERE book_id = ? AND user_login = ?");
        stmt.setInt(1, item.book.id);
        stmt.setString(2, item.user.login);
        return stmt;
    }

    @Override
    protected PreparedStatement getUpdateSQL(Connection conn, Reservation item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("UPDATE library.reservations SET book_id = ?, user_login = ? WHERE book_id = ? AND user_login = ?");
        stmt.setInt(1, item.book.id);
        stmt.setString(2, item.user.login);
        stmt.setInt(3, item.book.id);
        stmt.setString(4, item.user.login);
        return stmt;
    }
}
