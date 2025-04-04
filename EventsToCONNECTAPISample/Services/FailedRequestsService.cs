namespace EventsToCONNECTAPISample.Services
{
    public class FailedRequestsService
    {
        public List<string> FailedRequests { get; }

        public FailedRequestsService()
        {
            FailedRequests = new List<string>();
        }
    }
}
