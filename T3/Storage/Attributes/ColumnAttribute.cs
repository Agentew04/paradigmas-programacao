namespace BolsaValores.Storage.Attributes; 

/// <summary>
/// Specifies the name of the column on the database
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class ColumnAttribute : Attribute{
    public string Name { get; }
    public ColumnAttribute(string columnName) {
        Name = columnName;
    }
}