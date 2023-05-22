package Library.Collections;

import Library.Models.Loan;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class LoanCollection extends CollectionBase<Loan>{

    /**
     * Creates a new LoanCollection instance.
     *
     * @param conn The connection to the database
     */
    public LoanCollection(Connection conn) {
        super(conn);
    }

    @Override
    public Loan readResult(ResultSet rs) throws SQLException {
        BookCollection bookReader = new BookCollection(null);
        UserCollection userReader = new UserCollection(null);
        return new Loan(
                bookReader.readResult(rs),
                userReader.readResult(rs),
                rs.getTimestamp("retirada").toLocalDateTime()
        );
    }

    @Override
    protected PreparedStatement getSelectSQL(Connection conn) throws SQLException {
        return conn.prepareStatement("""
                select\s
                loans.retirada,\s
                books.title, books.id, books.author, books.publisher, books.isbn, books.edition, books.year,
                users.login, users.name, users.teacher
                from loans
                inner join books on livro_id = books.id
                inner join users on user_login = users.login;""");
    }

    @Override
    protected PreparedStatement getInsertSQL(Connection conn, Loan item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("INSERT INTO library.loans (livro_id, user_login, retirada) VALUES (?, ?, ?)");
        stmt.setInt(1, item.livro.id);
        stmt.setString(2, item.usuario.login);
        stmt.setTimestamp(3, java.sql.Timestamp.valueOf(item.retirada));
        return stmt;
    }

    @Override
    protected PreparedStatement getDeleteSQL(Connection conn, Loan item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("DELETE FROM library.loans WHERE livro_id = ? AND user_login = ?");
        stmt.setInt(1, item.livro.id);
        stmt.setString(2, item.usuario.login);
        return stmt;
    }

    @Override
    protected PreparedStatement getUpdateSQL(Connection conn, Loan item) throws SQLException {
        PreparedStatement stmt = conn.prepareStatement("UPDATE library.loans SET retirada = ? WHERE livro_id = ? AND user_login = ?");
        stmt.setTimestamp(1, java.sql.Timestamp.valueOf(item.retirada));
        stmt.setInt(2, item.livro.id);
        stmt.setString(3, item.usuario.login);
        return stmt;
    }
}
