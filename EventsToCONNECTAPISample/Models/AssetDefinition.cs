namespace EventsToCONNECTAPISample.Models
{
    public class AssetDefinition
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<MetadataDefinition> Metadata { get; } = new List<MetadataDefinition>();
    }
}
