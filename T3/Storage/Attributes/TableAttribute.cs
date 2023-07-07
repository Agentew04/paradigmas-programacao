namespace BolsaValores.Storage.Attributes; 

/// <summary>
/// Specifies the name of the table containing
/// the class on the database
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class TableAttribute : Attribute {
    public string Name { get; }

    public TableAttribute(string tableName) {
        Name = tableName;
    }
    
}