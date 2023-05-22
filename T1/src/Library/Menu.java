package Library;

import Library.Models.Book;
import Library.Models.Loan;
import Library.Models.Reservation;
import Library.Models.User;

import java.util.*;

public class Menu {
    public static int showMainMenu(){
        System.out.print("""
        -=-=-=-=-=-=-=-=-=-=-=-=-
        1. Livros (alterar, mudar, etc.)
        2. Usuários (adicionar, listar, etc.)
        3. Empréstimos
        4. Reserva
        5. Multas
        6. Sair
        >\s""");

        Scanner scanner = new Scanner(System.in);
        int option = scanner.nextInt();
        scanner.nextLine(); // consome o \n
        if(option < 1 || option > 6){
            System.out.println("Opção inválida!");
            return 0;
        }
        return option;
    }


    public static void startBookMenu(Library lib){
        Scanner scan = new Scanner(System.in);
        int choice;
        do{
            System.out.println("""
            -=-=-=-=-=-=-=-=-=-=-=-=-
            Funcoes para livros:
            \t1. Incluir
            \t2. Alterar
            \t3. Excluir
            \t4. Listar
            \t5. Buscar por ISBN
            \t6. Buscar por titulo
            \t7. Buscar por editora
            \t8. Voltar
            >\s""");
            choice = scan.nextInt();
            scan.nextLine(); // consome o \n

            switch (choice) {
                case 1 -> addBookInteractive(lib);
                case 2 -> editBookInteractive(lib);
                case 3 -> deleteBookInteractive(lib);
                case 4 -> listAllBooks(lib);
                case 5 -> findBookByIsbnInteractive(lib, scan);
                case 6 -> findBookByTitleInteractive(lib, scan);
                case 7 -> findBookByPublisherInteractive(lib, scan);
            }
        }while (choice != 8);
    }
    
    public static void startUserMenu(Library lib){
        Scanner scan = new Scanner(System.in);
        int choice;
        do{
            System.out.println("""
            -=-=-=-=-=-=-=-=-=-=-=-=-
            Funcoes para usuarios:
            \t1. Incluir
            \t2. Excluir
            \t3. Listar
            \t4. Buscar por parte do nome
            \t5. Buscar por login
            \t6. Voltar
            >\s""");
            choice = scan.nextInt();
            scan.nextLine(); // consome o \n

            switch (choice) {
                case 1 -> addUserInteractive(lib, scan);
                case 2 -> deleteUserInteractive(lib, scan);
                case 3 -> listUsersInteractive(lib);
                case 4 -> findUserByNameInteractive(lib, scan);
                case 5 -> findUserByLoginInteractive(lib, scan);
            }
        }while (choice != 6);
    }

    public static void startLoanMenu(Library lib){
        Scanner scan = new Scanner(System.in);
        int choice;
        do{
            System.out.println("""
            -=-=-=-=-=-=-=-=-=-=-=-=-
            Funcoes para emprestimos:
            \t1. Emprestar
            \t2. Renovar
            \t3. Devolver
            \t4. Listar
            \t5. Buscar emprestimos por livro(ISBN e titulo)
            \t6. Voltar
            >\s""");
            choice = scan.nextInt();
            scan.nextLine(); // consome o \n

            switch (choice){
                case 1 -> loanBookInteractive(lib, scan);
                case 2 -> renewLoanInteractive(lib, scan);
                case 3 -> returnLoanInteractive(lib, scan);
                case 4 -> listLoansInteractive(lib);
                case 5 -> findLoan(lib, scan);
            }
        }while (choice != 6);
    }

    public static void startReservationMenu(Library lib){
        Scanner scan = new Scanner(System.in);
        int choice;
        do{
            System.out.println("""
            -=-=-=-=-=-=-=-=-=-=-=-=-
            Funcoes para reservas:
            \t1. Solicitar
            \t2. Cancelar reserva
            \t3. Ver suas reservas
            \t4. Voltar
            >\s""");
            choice = scan.nextInt();
            scan.nextLine(); // consome o \n

            switch (choice) {
                case 1 -> makeReservationInteractive(lib);
                case 2 -> cancelReservationInteractive(lib);
                case 3 -> listResevationsInteractive(lib);
            }
        }while (choice != 4);
    }

    public static void startFineMenu(Library lib){
        Scanner scan = new Scanner(System.in);
        int choice;
        do{
            System.out.println("""
            -=-=-=-=-=-=-=-=-=-=-=-=-
            Funcoes para multas:
            \t1. Ver se voce tem multas
            \t2. Pagar multas
            \t3. Voltar
            >\s""");
            choice = scan.nextInt();
            scan.nextLine(); // consome o \n

            switch (choice) {
                case 1 -> checkFinesInteractive(lib);
                case 2 -> payFinesInteractive(lib);
            }
        }while (choice != 3);
    }

    private static void payFinesInteractive(Library lib) {
        int finesPaid = lib.fineAmount(null, true);
        if (finesPaid == 0) {
            System.out.println("Voce nao tinha multas para pagar.");
        } else {
            System.out.printf("Voce pagou R$%2d,00 de multas e devolveu os livros atrasados.\n", finesPaid);
        }
    }

    private static void checkFinesInteractive(Library lib) {
        int fines = lib.fineAmount(null, false);
        if (fines == 0) {
            System.out.println("Voce nao tem multas.");
        } else {
            System.out.printf("Voce tem R$%2d,00 de multas.\n", fines);
        }
    }

    private static void addBookInteractive(Library lib){
        Scanner scan = new Scanner(System.in);
        Book book = new Book();
        System.out.println("Digite as informações do livro: ");
        System.out.print("Titulo: "); book.title = scan.nextLine();
        System.out.print("Autor: "); book.author = scan.nextLine();
        System.out.print("Editora: "); book.publisher = scan.nextLine();
        System.out.print("Isbn: "); book.isbn = scan.nextLine();
        System.out.print("Ano: "); book.year = scan.nextInt();
        System.out.print("Edicao: "); book.edition = scan.nextInt();
        lib.addBook(book);
        System.out.println("Livro adicionado com sucesso! O id dele é "+book.id);
    }

    private static void editBookInteractive(Library lib){
        Scanner scan = new Scanner(System.in);
        System.out.println("Qual o id do livro quer mudar?");
        int id = scan.nextInt();
        Book book = lib.getBookById(id);
        System.out.println("Editando o livro de id " + id+"...");
        System.out.println("Vou listar os campos, deixe em branco se não deseja mudar.");
        System.out.print("Titulo: ");
        scan.nextLine(); // come o \n do id
        String newTitle = scan.nextLine();
        boolean editado = false;
        if(!Objects.equals(newTitle, "")){
            book.title = newTitle;
            editado = true;
        }

        System.out.print("Autor: ");
        String newAuthor = scan.nextLine();
        if(!Objects.equals(newAuthor, "")){
            book.author = newAuthor;
            editado = true;
        }

        System.out.print("Editora: ");
        String newPublisher = scan.nextLine();
        if(!Objects.equals(newPublisher, "")){
            book.publisher = newPublisher;
            editado = true;
        }

        System.out.print("ISBN: ");
        String newIsbn = scan.nextLine();
        if(!Objects.equals(newIsbn, "")){
            book.isbn = newIsbn;
            editado = true;
        }

        System.out.print("Edicao: ");
        String newEditionStr = scan.nextLine();
        if(!Objects.equals(newEditionStr, "")){
            book.edition = Integer.parseInt(newEditionStr);
            editado = true;
        }

        System.out.print("Ano: ");
        String newYearStr = scan.nextLine();
        if(!Objects.equals(newYearStr, "")){
            book.year = Integer.parseInt(newYearStr);
            editado = true;
        }

        if(editado){
            lib.updateBook(book);
            System.out.println("Livro editado com sucesso!");
        }else{
            System.out.println("Nenhum campo alterado.");
        }

    }

    private static void deleteBookInteractive(Library lib){
        Scanner scan = new Scanner(System.in);
        System.out.println("Digite o id do livro a ser deletado: ");
        int id = scan.nextInt();
        scan.nextLine(); // consume nextInt \n

        lib.removeBook(id);
        System.out.println("Livro removido com sucesso!");
    }

    private static void listAllBooks(Library lib) {
        System.out.println("Listando todos os livros...");
        List<Book> books = lib.listAllBooks();
        for(Book book : books){
            System.out.println(book);
        }
    }

    private static void makeReservationInteractive(Library lib) {
        Scanner scan = new Scanner(System.in);
        System.out.println("Digite o titulo completo do livro que quer reservar");

        System.out.print("Titulo: ");
        String title = scan.nextLine();

        boolean result = lib.makeReservation(title ,null);

        if(result){
            System.out.println("Livro reservado com sucesso!");
        }else{
            System.out.println("Nao foi possivel reservar seu livro :(");
            System.out.println("Ou voce tem multas a pagar ou nao existem copias disponiveis no momento");
        }
    }

    private static void cancelReservationInteractive(Library lib){
        Scanner scan = new Scanner(System.in);

        System.out.println("Digite o id do livro que deseja cancelar a reserva.");
        System.out.println("O id pode ser visto no menu de 'mostrar suas reservas'");

        int id = scan.nextInt();
        scan.nextLine(); //consome \n
        boolean result = lib.cancelReservation(id,null);
        if(result){
            System.out.println("A reserva do livro foi cancelada com sucesso!");
        }else{
            System.out.println("Nao foi possivel cancelar essa reserva.");
            System.out.println("Talvez voce digitou o id errado?");
        }
    }

    private static void listResevationsInteractive(Library lib){
        List<Reservation> reservations = lib.getReservations();
        for (Reservation r : reservations) {
            System.out.printf("id: %d, Titulo: %s\n", r.book.id, r.book.title);
        }
    }
    
    private static void findBookByPublisherInteractive(Library lib, Scanner scan) {
        System.out.println("Digite a editora: ");
        String publisher = scan.nextLine();
        Book bookPublisher = lib.getBookByPublisher(publisher);
        System.out.println("Este e o primeiro livro que achei desta editora: ");
        System.out.println(bookPublisher);
    }

    private static void findBookByTitleInteractive(Library lib, Scanner scan) {
        System.out.println("Digite uma parte do titulo: ");
        String title = scan.nextLine();
        Book bookTitle = lib.getBookByTitle(title);
        System.out.println(bookTitle);
    }

    private static void findBookByIsbnInteractive(Library lib, Scanner scan) {
        System.out.println("Digite o ISBN: ");
        String isbn = scan.nextLine();
        Book bookIsbn = lib.getBookByIsbn(isbn);
        System.out.println(bookIsbn);
    }

    private static void findUserByLoginInteractive(Library lib, Scanner scan) {
        System.out.println("Digite o login");
        String loginSearch = scan.nextLine();
        var userlogin = lib.getUserByLogin(loginSearch);
        if (userlogin == null) {
            System.out.println("Usuario nao encontrado");
        } else {
            System.out.println("Usuario encontrado: ");
            System.out.println(userlogin);
        }
    }

    private static void findUserByNameInteractive(Library lib, Scanner scan) {
        System.out.println("Digite uma parte do nome");
        String nameSearch = scan.nextLine();
        var nameUsers = lib.getUserByName(nameSearch);
        System.out.println("Usuarios encontrados: ");
        nameUsers.forEach(System.out::println);
    }

    private static void listUsersInteractive(Library lib) {
        List<User> allUsers = lib.listUsers();
        for (User user : allUsers) {
            System.out.println(user);
        }
    }

    private static void deleteUserInteractive(Library lib, Scanner scan) {
        System.out.println("Digite o login do usuario");
        String loginDelete = scan.nextLine();
        lib.removeUser(loginDelete);
        System.out.println("Usuario deletado com sucesso!");
    }

    private static void addUserInteractive(Library lib, Scanner scan) {
        System.out.println("Digite o nome completo");
        String name = scan.nextLine();
        System.out.println("Digite o login");
        String login = scan.nextLine();
        System.out.println("Ele e um professor? (s/n)");
        boolean teacher = scan.nextLine().toLowerCase(Locale.ROOT).equals("s");
        lib.registerUser(name, login, teacher);
        System.out.println("Usuario cadastrado com sucesso!");
    }
    private static void findLoan(Library lib, Scanner scan) {
        System.out.println("Digite o ISBN do livro: ");
        String isbn = scan.nextLine();
        System.out.println("Digite o titulo do livro: ");
        String title = scan.nextLine();

        var loans = lib.getLoansByIsbn(isbn, title);
        System.out.println("Emprestimos encontrados para esse livro: ");
        for (Loan loan : loans) {
            System.out.println(loan);
        }
    }

    private static void listLoansInteractive(Library lib) {
        var loans = lib.listLoans();
        System.out.println("Livros emprestados: ");
        if(loans.isEmpty()){
            System.out.println("Nenhum livro emprestado no momento :(");
        }else {
            for (Loan loan : loans) {
                System.out.println(loan.livro.title);
            }
        }
    }

    private static void returnLoanInteractive(Library lib, Scanner scan) {
        System.out.println("Digite o titulo do livro que deseja devolver");
        String title = scan.nextLine();
        boolean res = lib.returnLoan(title);
        if (!res) {
            System.out.println("Nao foi possivel devolver o seu livro :(");
        } else {
            System.out.println("Livro devolvido com sucesso!");
        }
    }

    private static void renewLoanInteractive(Library lib, Scanner scan) {
        var loans = lib.listLoans();
        System.out.println("Livros emprestados: ");
        for (Loan loan : loans) {
            System.out.println(loan.livro.title);
        }
        System.out.println("Digite o titulo do livro que deseja renovar");
        String title = scan.nextLine();
        boolean result = lib.renewLoan(title);
        if (!result) {
            System.out.println("Nao foi possivel renovar o seu livro :(");
        } else {
            System.out.println("Livro renovado com sucesso!");
        }
    }

    private static void loanBookInteractive(Library lib, Scanner scan) {
        System.out.println("Digite o titulo completo");
        String title = scan.nextLine();
        var res = lib.loanBook(title); // funcao printa erro
        if(res){
            System.out.println("Livro emprestado com sucesso!");
        }
    }
}
