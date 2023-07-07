using BolsaValores.Storage.Attributes;

namespace BolsaValores.Models;
 
[Serializable]
[Table("ativos")]
public class Asset {

    [PrimaryKey] 
    [Column(nameof(ticker))] 
    public string ticker;

    [Column(nameof(nome))] 
    public string nome;

    [Column(nameof(tipo))]
    public int tipo;
}