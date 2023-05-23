using BolsaValores.Exceptions;

namespace BolsaValores; 

public class Menu {
    private WalletManager _walletManager;
    
    public Menu(WalletManager walletManager) {
        _walletManager = walletManager;
    }

    private void ShowMainMenuWelcome() {
        Console.WriteLine(@"
Bem-vindo ao sistema da Bolsa de Valores!
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
                    Console.Clear();
                    
                    Console.WriteLine("Qual vai ser o seu login? ");
                    Console.Write("> ");
                    string newLogin = Console.ReadLine() ?? "";
                    bool success = true;
                    try {
                        _walletManager.AddWallet(newLogin);
                    }
                    catch (WalletExistsException ex) {
                        ConsoleEx.Error(ex.Message);
                        success = false;
                    }
                    catch (InvalidWalletNameException ex) {
                        ConsoleEx.Error(ex.Message);
                        success = false;
                    }

                    if (success)
                        ConsoleEx.Write("A sua carteira foi criada com sucesso!", ConsoleColor.Green);
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    string accessLogin = Console.ReadLine() ?? "";
                    var wallet = _walletManager.GetWallet(accessLogin);
                    if(wallet)
                    Console.WriteLine("Acessando a carteira...");
                    Console.WriteLine("fim acesso carteira");
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    Console.WriteLine("Até mais!");
                    exit = true;
                    break;
                default:
                    Console.Clear();
                    ConsoleEx.Error("Opção inválida!");
                    break;
            }
            Thread.Sleep(1000);
        }
    }
}