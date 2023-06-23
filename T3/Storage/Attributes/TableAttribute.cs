namespace BolsaValores.Storage.Attributes; 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class TableAttribute : Attribute {
    public string Name { get; }

    public TableAttribute(string tableName) {
        Name = tableName;
    }
    
}