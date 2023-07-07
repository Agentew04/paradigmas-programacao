namespace BolsaValores.Storage.Attributes; 

/// <summary>
/// Marks a field or a property as a primary key on
/// the database
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class PrimaryKeyAttribute : Attribute {
    
}