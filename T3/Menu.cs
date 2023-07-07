using System.Reflection.Metadata.Ecma335;
using BolsaValores.Exceptions;
using BolsaValores.Models;
using BolsaValores.Storage;

namespace BolsaValores; 

/// <summary>
/// Classe que gerencia o fluxo do programa e a lógica de negócios
/// </summary>
public class Menu {
    private readonly Crud<Asset> _assetCrud = new();
    private readonly Crud<AssetType> _typeCrud = new();
    private readonly Crud<AssetValue> _valueCrud = new();
    private readonly Crud<WalletOperation> _walletCrud = new();
    
    #region Metodos publicos

    /// <summary>
    /// Começa o menu principal
    /// </summary>
    public void StartMenu() {
        Console.WriteLine(@"
Bem-vindo ao sistema da Bolsa de Valores! O que voce gostaria de fazer hoje?");
        var exit = false;
        
        while (!exit) {
            Console.WriteLine("""
                1. Comprar/incluir ativos
                2. Mostrar todos ativos disponíveis
                3. Mostrar rentabilidade total da carteira
                4. Percentual de ganho/perda de um ativo
                5. O valor de ganho/perda de um ativo
                6. Preço médio de aquisição de um ativo
                7. Sair
            """);
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key) {
                case ConsoleKey.D1:
                    BuyAsset();
                    break;
                case ConsoleKey.D2:
                    ShowAllAssets();
                    break;
                case ConsoleKey.D3:
                    ShowWalletProfitability();
                    break;
                case ConsoleKey.D4:
                    ShowAssetProfitability();
                    break;
                case ConsoleKey.D5:
                    ShowAssetProfit();
                    break;
                case ConsoleKey.D6:
                    ShowAssetAverageBuyPrice();
                    break;
                case ConsoleKey.D7:
                    exit = true;
                    break;
                default:
                    ConsoleEx.Error("Opção inválida!");
                    break;
            }

            Thread.Sleep(1000);
        }
    }

    

    #endregion

    #region Metodos privados

    /// <summary>
    /// Compra um ativo e o adiciona à carteira
    /// </summary>
    private void BuyAsset() {
        Console.WriteLine("Ok! Vamos comprar um ativo. Digite o ticker do ativo que você deseja comprar:");
        Console.Write("> ");
        string ticker = (Console.ReadLine() ?? "").ToUpper();
        
        if (!_assetCrud.Select(x => x.ticker == ticker).Any()) {
            ConsoleEx.Error("Esse ticker não existe!");
            return;
        }
        
        Console.WriteLine("Quantos você vai comprar?");
        Console.Write("> ");
        string quantityStr = Console.ReadLine() ?? "";
        if (!int.TryParse(quantityStr, out int quantity) || quantity <= 0) {
            ConsoleEx.Error("Essa quantidade é inválida!");
            return;
        }
        
        Console.WriteLine("Qual o preço de compra?");
        Console.Write("> ");
        string priceStr = Console.ReadLine() ?? "";
        if (!float.TryParse(priceStr, out float price) || price <= 0) {
            ConsoleEx.Error("Essa quantidade é inválida!");
            return;
        }

        var lastId = _walletCrud.GetLast<int>("id");
        
        WalletOperation wo = new() {
            id = lastId+1,
            ticker = ticker,
            data = DateTime.Today,
            operacao = 'C',
            preco = price,
            quantidade = quantity
        };
        
        _walletCrud.Add(wo);
        
        ConsoleEx.Success("Ativo comprado com sucesso!");
    }

    /// <summary>
    /// Mostra todos os ativos existentes ao usuário
    /// </summary>
    private void ShowAllAssets() {
        Console.WriteLine("Ok! Vamos mostrar todos os ativos disponíveis.");
        ConsoleEx.Warn("Recuperando lista de ativos...");
        IEnumerable<Asset> assets = _assetCrud.Select();
        foreach (Asset asset in assets) {
            string nomeTipo = _typeCrud.Select(x => x.id == asset.tipo).First().tipo;
            Console.WriteLine($"{asset.ticker} - {asset.nome} - {nomeTipo}");
        }
        if(assets.Any())
            ConsoleEx.Success("Listagem concluída!");
        else
            ConsoleEx.Warn("Não há ativos cadastrados!");
    }
    
    /// <summary>
    /// Calcular a porcentagem de rentabilidade da carteira
    /// dados todas as operações realizadas e os preços históricos
    /// de cada ativo.
    /// </summary>
    private void ShowWalletProfitability() {
        Console.WriteLine("Ok! Vamos mostrar a rentabilidade da sua carteira.");
        ConsoleEx.Warn("Recuperando lista de operações...");
        
        IEnumerable<WalletOperation> operations = _walletCrud.Select();
        IEnumerable<AssetValue> values = _valueCrud.Select();

        if (!operations.Any()) {
            ConsoleEx.Error($"Você nao incluiu nenhum ativo na sua carteira!");
            return;
        }

        float precoPagoTotal = operations.Sum(op => op.preco * op.quantidade);

        float precoAtualTotal = operations.Sum(op => {
            float valorAtual = values.Where(x => x.ticker == op.ticker)
                .OrderByDescending(x => x.dia)
                .First().valor;
            return op.quantidade * valorAtual;
        });
        
        float rentabilidade = precoAtualTotal/precoPagoTotal - 1;

        ConsoleEx.Success($"A rentabilidade da sua carteira é de {rentabilidade:P2}");
    }
    
    /// <summary>
    /// Mostra o valor de ganho/perda de um ativo
    /// </summary>
    private void ShowAssetProfit() {
        Console.WriteLine("Qual o ticker do ativo que você deseja ver a rentabilidade?");
        Console.Write("> ");
        string ticker = (Console.ReadLine() ?? "").ToUpper();
        
        if(!_assetCrud.Select(x => x.ticker == ticker).Any()) {
            ConsoleEx.Error("Esse ticker não existe!");
            return;
        }

        ConsoleEx.Warn("Recuperando lista de operações...");
        IEnumerable<WalletOperation> operations = _walletCrud.Select(x => x.ticker == ticker);
        
        if (!operations.Any()) {
            ConsoleEx.Error($"Você não tem nenhuma compra desse ativo!");
            return;
        }
        
        float ultimoPreco = (from value in _valueCrud.Select()
                             where value.ticker == ticker
                             orderby value.dia descending
                             select value.valor).First();
        float lucro = (from op in operations
                       let pago = op.preco * op.quantidade
                       let atual = op.quantidade * ultimoPreco
                       select atual - pago).Sum();

        ConsoleEx.Success($"{(lucro > 0 ? "O lucro" : "A perda")} dos seus ativos {ticker} é de {MathF.Abs(lucro):C2}");
    }
    
    /// <summary>
    /// Calcula a rentabilidade de um ativo específico
    /// </summary>
    private void ShowAssetProfitability() {
        Console.WriteLine("Qual o ticker do ativo que você deseja ver a rentabilidade?");
        Console.Write("> ");
        string ticker = (Console.ReadLine() ?? "").ToUpper();

        if(!_assetCrud.Select(x => x.ticker == ticker).Any()) {
            ConsoleEx.Error("Esse ticker não existe!");
            return;
        }
        
        ConsoleEx.Warn("Recuperando lista de operações...");
        IEnumerable<WalletOperation> operations = _walletCrud.Select(x => x.ticker == ticker);
        
        if (!operations.Any()) {
            ConsoleEx.Error($"Você não tem nenhuma compra desse ativo!");
            return;
        }
        
        float precoPagoTotal = operations.Sum(op => op.preco * op.quantidade);
        float ultimoPreco = (from value in _valueCrud.Select()
                             where value.ticker == ticker
                             orderby value.dia descending
                             select value.valor).First();
        float precoAtualTotal = operations.Sum(op => op.quantidade * ultimoPreco);
        float rentabilidade = precoAtualTotal/precoPagoTotal - 1;
        
        ConsoleEx.Success($"A rentabilidade dos seus ativos {ticker} é de {rentabilidade:P2}");
    }
    
    /// <summary>
    /// Calcula o preço médio de compra de um ativo
    /// </summary>
    private void ShowAssetAverageBuyPrice() {
        Console.WriteLine("Qual o ticker do ativo que você deseja ver a rentabilidade?");
        Console.Write("> ");
        string ticker = (Console.ReadLine() ?? "").ToUpper();
        
        if(!_assetCrud.Select(x => x.ticker == ticker).Any()) {
            ConsoleEx.Error("Esse ticker não existe!");
            return;
        }
        
        ConsoleEx.Warn("Recuperando lista de operações...");
        IEnumerable<WalletOperation> operations = _walletCrud.Select(x => x.ticker == ticker);
        
        if (!operations.Any()) {
            ConsoleEx.Error($"Você não tem nenhuma compra desse ativo!");
            return;
        }

        double precoUnidadeMedia = operations.Average(x => x.preco);
        double precoCompraMedia = operations.Average(x => x.preco * x.quantidade);
        
        ConsoleEx.Success($"O preço médio por unidade do ativo {ticker} é de {precoUnidadeMedia:C2}");
        ConsoleEx.Success($"O preço médio de compra do ativo {ticker} é de {precoCompraMedia:C2}");
    }
    
    #endregion

}