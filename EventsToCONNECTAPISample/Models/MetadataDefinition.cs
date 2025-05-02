namespace EventsToCONNECTAPISample.Models
{
    public class MetadataDefinition
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int SdsTypeCode { get; set; }
    }

    public static class SdsTypeCodes
    {
        public const int SdsInt = 11;
        public const int SdsDouble = 14;
        public const int SdsDateTime = 16;
        public const int SdsString = 18;
    }
}
