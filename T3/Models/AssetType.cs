using BolsaValores.Storage.Attributes;

namespace BolsaValores.Models; 

[Serializable]
[Table("tipos")]
public class AssetType {

    [PrimaryKey]
    [Column(nameof(id))]
    public int id;

    [Column(nameof(tipo))]
    public string tipo;
}