package Library.Models;

public class User {
    public static final int STUDENT_MAX_LOAN_TIME = 7;
    public static final int STUDENT_MAX_LOAN_BOOKS = 3;
    public static final int TEACHER_MAX_LOAN_TIME = 15;
    public static final int TEACHER_MAX_LOAN_BOOKS = 5;

    public String name;

    public String login;


    public boolean isTeacher;

    public User(String name, String login){
        this.name = name;
        this.login = login;
    }

    public User(){}

    @Override
    public boolean equals(Object obj) {
        if(obj instanceof User u){
            return this.login.equals(u.login) && this.name.equals(u.name);
        }
        return false;
    }

    @Override
    public String toString() {
        return "User [login=" + login + ", name=" + name + "]";
    }
}
