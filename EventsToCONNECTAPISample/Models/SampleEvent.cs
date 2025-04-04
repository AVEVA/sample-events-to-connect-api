namespace EventsToCONNECTAPISample.Models
{
    public class SampleEvent
    {
        public string? Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Sample { get; set; }
        public SiteReference? Site { get; set; }
    }

    public class SiteReference
    {
        public string? Id { get; set; }
    }
}
