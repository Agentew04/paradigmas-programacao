using BolsaValores.Storage.Attributes;

namespace BolsaValores;
 
[Serializable]
[Table("ativos")]
public class Asset {
    
    [PrimaryKey]
    [Column("ticker")]
    public string Ticker { get; set; }
    
    [Column("empresa")]
    public string Nome { get; set; }
}