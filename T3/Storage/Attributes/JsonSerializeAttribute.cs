namespace BolsaValores.Storage.Attributes; 

/// <summary>
/// Marks that a field or a property should be serialized as json
/// on the database. Uses the System.Text.Json library.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class JsonSerializeAttribute : Attribute {
    
}