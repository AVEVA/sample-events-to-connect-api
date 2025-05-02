namespace EventsToCONNECTAPISample.Models
{
    public class TypeDefinition
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? DefaultAuthorizationTag { get; set; }
        public List<PropertyDefinition> Properties { get; } = new List<PropertyDefinition>();
    }
}
