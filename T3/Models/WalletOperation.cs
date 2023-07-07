using BolsaValores.Models;
using BolsaValores.Storage.Attributes;

namespace BolsaValores; 


[Serializable]
[Table("carteira")]
public class WalletOperation {

    [PrimaryKey] 
    [Column("id")] 
    public int id;

    [Column(nameof(operacao))]
    public char operacao;

    [Column(nameof(data))]
    public DateTime data;

    [Column(nameof(ticker))]
    public string ticker;
    
    [Column(nameof(quantidade))]
    public int quantidade;
    
    [Column(nameof(preco))]
    public float preco;
}