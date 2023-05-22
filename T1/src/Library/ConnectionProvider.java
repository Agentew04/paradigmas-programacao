package Library;

import java.sql.Connection;
import java.sql.DriverManager;

public class ConnectionProvider {
    static final String serverName = "localhost:3306";
    static final String databaseName = "library";
    static final String username = "root";
    static final String password = "abc12345";

    private static Connection conn = null;

    public static Connection getConn(){
        if(conn == null){
            conn = openNewConnection();
        }
        return conn;
    }

    private static Connection openNewConnection(){
        Connection conn;
        try {
            conn = DriverManager.getConnection(url(), username, password);
        }catch (Exception ex){
            System.out.println(ex.getMessage());
            return null;
        }
        return conn;
    }

    private static String url(){
        return "jdbc:mysql://" + serverName + "/" + databaseName;
    }


}
