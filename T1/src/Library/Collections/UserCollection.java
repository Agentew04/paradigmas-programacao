package Library.Collections;

import Library.Models.User;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class UserCollection extends CollectionBase<User>{
    /**
     * Creates a new CollectionBase instance.
     *
     * @param conn     The connection to the database
     */
    public UserCollection(Connection conn) {
        super(conn);
    }

    @Override
    public User readResult(ResultSet rs) throws SQLException {
        User u = new User();
        u.login = rs.getString("login");
        u.name = rs.getString("name");
        u.isTeacher = rs.getBoolean("teacher");
        return u;
    }

    @Override
    protected PreparedStatement getSelectSQL(Connection conn) throws SQLException {
        return conn.prepareStatement("SELECT * FROM library.users");
    }

    @Override
    protected PreparedStatement getInsertSQL(Connection conn, User item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("INSERT INTO library.users (login, name, teacher) VALUES (?, ?, ?)");
        stmt.setString(1, item.login);
        stmt.setString(2, item.name);
        stmt.setBoolean(3, item.isTeacher);
        return stmt;
    }

    @Override
    protected PreparedStatement getDeleteSQL(Connection conn, User item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("DELETE FROM library.users WHERE login = ?");
        stmt.setString(1, item.login);
        return stmt;
    }

    @Override
    protected PreparedStatement getUpdateSQL(Connection conn, User item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("UPDATE library.users SET name = ?, teacher = ? WHERE login = ?");
        stmt.setString(1, item.name);
        stmt.setBoolean(2, item.isTeacher);
        stmt.setString(3, item.login);
        return stmt;
    }
}
