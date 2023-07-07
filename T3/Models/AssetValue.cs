using BolsaValores.Storage.Attributes;

namespace BolsaValores.Models; 

[Serializable]
[Table("cotacao")]
public class AssetValue {

    [PrimaryKey] 
    [Column(nameof(ticker))] 
    public string ticker;

    [Column(nameof(dia))] 
    public DateTime dia;

    [Column(nameof(valor))] 
    public float valor;
}