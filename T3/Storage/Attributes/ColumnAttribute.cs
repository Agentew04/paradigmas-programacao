namespace BolsaValores.Storage.Attributes; 

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ColumnAttribute : Attribute{
    public string Name { get; }
    public ColumnAttribute(string columnName) {
        Name = columnName;
    }
}