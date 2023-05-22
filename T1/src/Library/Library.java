package Library;

import Library.Collections.BookCollection;
import Library.Collections.LoanCollection;
import Library.Collections.ReservationCollection;
import Library.Collections.UserCollection;
import Library.Models.Book;
import Library.Models.Loan;
import Library.Models.Reservation;
import Library.Models.User;

import java.time.LocalDateTime;
import java.time.temporal.ChronoUnit;
import java.util.*;

public class Library {
    private final BookCollection livros;
    private final UserCollection usuarios;
    private final LoanCollection emprestimos;
    private final ReservationCollection reservas;

    private User loggedUser = null;

    private final static int FINE_PER_DAY = 1;

    public Library(){
        livros = new BookCollection(ConnectionProvider.getConn());
        usuarios = new UserCollection(ConnectionProvider.getConn());
        emprestimos = new LoanCollection(ConnectionProvider.getConn());
        reservas = new ReservationCollection(ConnectionProvider.getConn());
    }

    /**
     * Loga um usuario na biblioteca
     * @param name O nome do usuario
     * @param login O login unico do usuario
     */
    public boolean login(String name, String login){
        User userInput = new User(name, login);

        User found = usuarios.getOne(x -> x.login.equals(login));
        if(found == null){
            Scanner scanner = new Scanner(System.in);
            System.out.println("Usuario nao encontrado. Voce e um professor? (s/n)");
            String isTeacher = scanner.nextLine();
            userInput.isTeacher = isTeacher.toLowerCase(Locale.ROOT).equals("s");
            usuarios.insert(userInput);
            return true;
        }

        if(found.login.equals(login) && !found.name.equals(name)){
            System.out.println("Nome de usuario incorreto");
            return false;
        }

        loggedUser = found;
        return true;
    }


    public void registerUser(String name, String login, boolean teacher){
        User user = new User();
        user.name = name;
        user.login = login;
        user.isTeacher = teacher;
        if(!usuarios.has(x -> x.login.equals(login))){
            usuarios.insert(user);
        }
    }
    public void removeUser(String login){
        usuarios.delete(x -> x.login.equals(login));
    }

    public List<User> listUsers(){
        return usuarios.get();
    }

    public List<User> getUserByName(String name){
        String invariantName = name.toLowerCase();
        return usuarios.get(
                x -> x.name.toLowerCase().contains(invariantName)
        );
    }

    public User getUserByLogin(String login){
        return usuarios.getOne(x -> x.login.equals(login));
    }

    public boolean loanBook(String title){
        List<Loan> loans = emprestimos.get(x -> x.usuario.login.equals(loggedUser.login));

        if(loans.size() > (loggedUser.isTeacher ?
        User.TEACHER_MAX_LOAN_BOOKS
        : User.STUDENT_MAX_LOAN_BOOKS )){
            System.out.println("Voce ja tem livros emprestados demais!");
            return false;
        }

        boolean isAlreadyLoaned = emprestimos.has(x -> x.livro.title.equals(title));
        if(isAlreadyLoaned){
            System.out.println("Voce ja tem esse livro emprestado ou alguem ja emprestou esse livro!");
            return false;
        }

        boolean isReserved = reservas.has(x -> !x.book.title.equals(title) &&
                        !x.user.login.equals(loggedUser.login));

        if (isReserved){
            System.out.println("O livro ja esta reservado para outro usuario");
            return false;
        }

        Loan l = new Loan();
        l.usuario = loggedUser;
        l.livro = livros.getOne(x-> x.title.equals(title));
        l.retirada = LocalDateTime.now();
        emprestimos.insert(l);
        return true;
    }

    public boolean renewLoan(String title){
        Book book = emprestimos.stream()
                .filter(x -> x.usuario.login.equals(loggedUser.login))
                .filter(x -> x.livro.title.equals(title))
                .map(x -> x.livro)
                .findFirst()
                .orElse(null);

        if(isReserved(book)){
            System.out.println("O livro ja esta reservado");
            return false;
        }
        LocalDateTime now = LocalDateTime.now();
        Optional<Loan> l = emprestimos.stream().filter(x-> x.usuario.login.equals(loggedUser.login))
                .filter(x -> x.livro.title.equals(title))
                .findFirst();
        l.ifPresent(x -> x.retirada = now);
        if(l.isPresent()) {
            emprestimos.update(l.get()  );
            return true;
        }
        return false;
    }

    public boolean returnLoan(String title){
        int deleted = emprestimos.delete(x -> x.usuario.login.equals(loggedUser.login) && x.livro.title.equals(title));
        return deleted > 0;
    }

    /**
     * Retorna todos os emprestimos do usuario logado
     * @return Uma lista de emprestimos
     */
    public List<Loan> listLoans(){
        return emprestimos.stream()
                .filter(x -> x.usuario.login.equals(loggedUser.login))
                .toList();
    }

    public List<Loan> getLoansByIsbn(String isbn, String title){
        return emprestimos.stream()
                .filter(x -> x.livro.isbn.equals(isbn))
                .filter(x -> x.livro.title.equals(title))
                .toList();
    }

    public boolean makeReservation(String title, User user){
        if(user == null){
            user = loggedUser;
        }

        if(hasFine(user)) {
            return false;
        }

        var books = livros.get(x -> x.title.equals(title));
        var availableBooks = books.stream()
                .filter(x -> !isReserved(x))
                .filter(x -> !isLoaned(x))
                .toList();

        if(availableBooks.isEmpty()){
            return false;
        }

        if(hasFine(loggedUser)){
            return false;
        }

        Reservation r = new Reservation();
        r.book = availableBooks.get(0);
        r.user = user;
        reservas.insert(r);
        return true;
    }


    public boolean cancelReservation(int id, User user){
        if(user == null){
            user = loggedUser;
        }

        User finalUser = user;
        Optional<Reservation> reservation = reservas.stream()
                .filter(x -> x.book.id == id)
                .filter(x -> x.user.login.equals(finalUser.login))
                .findFirst();

        if(reservation.isPresent()){
            reservas.delete(reservation.get());
            return true;
        }
        return false;

    }

    /**
     * Mostra todas as reservas do usuario logado na biblioteca
     * @return Uma lista com as reservas do usuario
     */
    public List<Reservation> getReservations(){
        return reservas.stream()
                .filter(x -> x.user.login.equals(loggedUser.login))
                .toList();
    }

    /**
     * Paga as multas e automaticamente devolve os empréstimos atrasados
     * @param user O usuario que vai pagar as multas. Se nulo assume o usuario logado
     * @return Retorna quantos reais de multas foram pagos
     */
    public int fineAmount(User user, boolean pay){
        int totalFines = 0;
        List<Loan> toRemove = new ArrayList<>();
        if(user == null){
            user = loggedUser;
        }
        int maxLoanDays = user.isTeacher ? User.TEACHER_MAX_LOAN_TIME : User.STUDENT_MAX_LOAN_TIME;
        for(Loan l : emprestimos){
            if(!l.usuario.equals(user)){
                continue;
            }
            long daysLate = l.retirada.until(LocalDateTime.now(), ChronoUnit.DAYS) - maxLoanDays;
            if( daysLate <= 0){
                continue;
            }
            toRemove.add(l);
            totalFines += FINE_PER_DAY * daysLate;
        }
        // remove os que estavam vencidos ja apos pagar a multa
        if(pay) {
            for (Loan l : toRemove) {
                emprestimos.delete(l);
            }
        }
        return totalFines;
    }

    /**
     * Verifica se o usuario tem multas pendentes
     * @param user O usuario a ser verificado
     * @return True se ele tem multas, senão false.
     */
    public boolean hasFine(User user){
        for (Loan loan : emprestimos){
            if(!loan.usuario.login.equals(user.login)){
                continue;
            }

            if(loan.retirada.until(LocalDateTime.now(), ChronoUnit.DAYS) >
                    (user.isTeacher ? User.TEACHER_MAX_LOAN_TIME : User.STUDENT_MAX_LOAN_TIME)){
                return true;
            }
        }
        return false;
    }

    /**
     * Verifica se um livro ja esta reservado
     * @param book O livro a ser verificado
     * @return {@code true} se esta reservado, senao {@code false}.
     */
    public boolean isReserved(Book book){
        for(Reservation r : reservas){
            if(r.book.equals(book)){
                return true;
            }
        }
        return false;
    }

    /**
     * Verifica se um livro está atualmente sob empréstimo
     * @param book O livro a ser analisado
     * @return O valor encontrado
     */
    public boolean isLoaned(Book book){
        for(Loan l : emprestimos){
            if(l.livro == book){
                return true;
            }
        }
        return false;
    }

    /**
     * Adiciona um livro a biblioteca.
     * @param book O livro a ser adicionado
     */
    public void addBook(Book book){
        livros.insert(book);
    }

    public void updateBook(Book book){
        livros.update(book);
    }

    public void removeBook(int id){
        Book b = new Book();
        b.id = id;
        livros.delete(b);
    }

    public List<Book> listAllBooks(){
        return livros.get();
    }

    public Book getBookById(int id){
        return livros.getOne(x -> x.id == id);
    }

    public Book getBookByIsbn(String isbn){
        return livros.getOne(x -> x.isbn.equals(isbn));
    }

    public Book getBookByTitle(String title) {
        String invariantTitle = title.toLowerCase();
        return livros.getOne(
                x -> x.title.toLowerCase().contains(invariantTitle));
    }

    public Book getBookByPublisher(String publisher){
        return livros.getOne(x -> x.publisher.equals(publisher));
    }
}
