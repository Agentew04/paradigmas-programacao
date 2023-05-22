package Library.Models;

import java.time.LocalDateTime;

public class Loan {
    public Book livro;
    public User usuario;
    public LocalDateTime retirada;

    public Loan(Book livro, User usuario, LocalDateTime retirada){
        this.livro = livro;
        this.usuario = usuario;
        this.retirada = retirada;
    }
    public Loan(){}

    @Override
    public boolean equals(Object obj) {
        if(obj instanceof Loan l){
            return this.livro.equals(l.livro) && this.usuario.equals(l.usuario) && this.retirada.equals(l.retirada);
        }
        return false;
    }

    @Override
    public String toString() {
        return "Loan [livro=" + livro + ", usuario=" + usuario + ", retirada=" + retirada + "]";
    }
}
