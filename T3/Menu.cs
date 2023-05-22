using System.Drawing;

namespace BolsaValores; 

public class Menu {
    
    public Menu() {
                
    }

    private void ShowMainMenuWelcome() {
        Console.WriteLine(@"Bem-vindo ao sistema da Bolsa de Valores!
Você gostaria de criar uma carteira ou acessar uma existente?");
    }

    private void ShowMainMenuOptions() {
        Console.WriteLine(@"    1. Criar carteira
    2. Acessar existente
    3. Sair");
    }

    public void StartMenu() {
        Console.Clear();
        ShowMainMenuWelcome();
        bool exit = false;
        while (!exit) {
            ShowMainMenuOptions();
            var key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.D1:
                    // do stuff
                    Console.Clear();
                    break;
                case ConsoleKey.D2:
                    // do stuff
                    Console.Clear();
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    Console.WriteLine("Até mais!");
                    exit = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida!");
                    Console.ResetColor();
                    break;
            }
        }
    }
}