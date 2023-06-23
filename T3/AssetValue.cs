using System.Reflection;
using BolsaValores.Storage.Attributes;

namespace BolsaValores.Storage; 

[Serializable]
[Table("cotacao")]
public class AssetValue {
    
    [PrimaryKey]
    [Column("ticker")]
    public string Ticker { get; set; }
    
    [Column("dia")]
    public DateTime Dia { get; set; }
    
    [Column("valor")]
    public float Valor { get; set; }
}