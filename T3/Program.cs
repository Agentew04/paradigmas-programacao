using BolsaValores.Storage;
using MySqlConnector;

namespace BolsaValores;

public static class Program {
    public static void Main(string[] args) {
        WalletManager walletManager = new();
        Menu menu = new(walletManager);
        menu.StartMenu();

        Console.WriteLine(SqlFactory.GetSelectSql(typeof(AssetValue)));
        Console.WriteLine(SqlFactory.GetInsertSql(typeof(AssetValue)));
        Console.WriteLine(SqlFactory.GetUpdateSql(typeof(AssetValue)));
        Console.WriteLine(SqlFactory.GetDeleteSql(typeof(AssetValue)));

        BaseCrud<AssetValue> crud = new();
        foreach (AssetValue assetValue in crud.Select()) {
            Console.Write(assetValue.Ticker + " ");
            Console.Write(assetValue.Dia + " ");
            Console.Write(assetValue.Valor);
            Console.WriteLine();
        }
    }
}