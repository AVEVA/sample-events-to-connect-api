namespace EventsToCONNECTAPISample.Models
{
    public class Asset
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AssetTypeId { get; set; }
        public List<MetadataItem> Metadata { get; } = new List<MetadataItem>();
    }
}
