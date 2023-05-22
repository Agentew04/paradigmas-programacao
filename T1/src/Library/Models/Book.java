package Library.Models;

import java.util.Objects;

public class Book {
    public String author;

    public String publisher;

    public String title;

    public String isbn;

    public int edition;

    public int year;

    public int id;

    public Book(String title, String author, String publisher, String isbn, int edition, int year, int id){
        this.title = title;
        this.author = author;
        this.publisher = publisher;
        this.isbn = isbn;
        this.edition = edition;
        this.year = year;
        this.id = id;
    }

    public Book(){

    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Book book = (Book) o;
        return edition == book.edition && year == book.year && id == book.id && author.equals(book.author) && publisher.equals(book.publisher) && title.equals(book.title) && isbn.equals(book.isbn);
    }

    @Override
    public int hashCode() {
        return Objects.hash(author, publisher, title, isbn, edition, year, id);
    }

    @Override
    public String toString(){
        StringBuilder sb = new StringBuilder();
        sb.append("id: %d\n".formatted(this.id));
        sb.append("\tAutor: %s\n".formatted(this.author));
        sb.append("\tTitulo: %s\n".formatted(this.title));
        sb.append("\tEditora: %s\n".formatted(this.publisher));
        sb.append("\tISBN: %s\n".formatted(this.isbn));
        sb.append("\tEdicao: %d\n".formatted(this.edition));
        sb.append("\tAno: %d\n".formatted(this.year));
        return sb.toString();
    }
}
