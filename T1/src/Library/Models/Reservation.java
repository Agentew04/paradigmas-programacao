package Library.Models;

public class Reservation {
    public Book book;
    public User user;

    public Reservation(Book book, User user){
        this.book = book;
        this.user = user;
    }

    public Reservation(){}
 }
