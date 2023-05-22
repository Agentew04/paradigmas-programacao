package Library;

import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
        }catch (ClassNotFoundException e){
            System.out.println("""
                    Nao achei a classe do MySQL!
                    "Verifique se o driver esta no classpath.
                    "Mensagem de erro:\s""" + e.getMessage());
            return;
        }

        Library lib = new Library();

        boolean loginSuccess;
        do {
            Scanner scan = new Scanner(System.in);
            System.out.println("Qual e seu nome?");
            String name = scan.nextLine();
            System.out.println("Qual e o seu login?");
            String login = scan.nextLine();
            loginSuccess = lib.login(name, login);
        }while (!loginSuccess);

        int choice;
        do{
            choice = Menu.showMainMenu();
            switch (choice) {
                case 1 -> Menu.startBookMenu(lib);
                case 2 -> Menu.startUserMenu(lib);
                case 3 -> Menu.startLoanMenu(lib);
                case 4 -> Menu.startReservationMenu(lib);
                case 5 -> Menu.startFineMenu(lib);
            }

        }while (choice != 6);
        System.out.println("Saindo...");
    }
}