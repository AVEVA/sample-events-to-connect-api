namespace EventsToCONNECTAPISample.Models
{
    public class PumpEvent
    {
        public string? Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? PumpStatus { get; set; }
        public Reference? Site { get; set; }
        public Reference? Pump { get; set; }
    }

    public class Reference
    {
        public string? Id { get; set; }
    }
}
