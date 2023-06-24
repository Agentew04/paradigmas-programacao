using BolsaValores.Storage;
using MySqlConnector;

namespace BolsaValores;

public static class Program {
    public static void Main(string[] args) {
        WalletManager walletManager = new();
        Menu menu = new(walletManager);
        menu.StartMenu();

    }
}