using EventsToCONNECTAPISample.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventsToCONNECTAPISampleTests
{
    public class UnitTests
    {
        private EventsService EventsService { get; set; }
        private FailedRequestsService FailedRequestsService { get; set; }

        public UnitTests()
        {
            // Build test services
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<EventsService>();
            serviceCollection.AddSingleton<FailedRequestsService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            EventsService = serviceProvider.GetService<EventsService>()!;
            FailedRequestsService = serviceProvider.GetService<FailedRequestsService>()!;
        }

        [Fact]
        public void EventsServiceShouldHaveEvents()
        {
            // Assert
            Assert.NotNull(EventsService.Events);
        }

        [Fact]
        public void FailedRequestsServiceShouldHandleFailedRequest()
        {
            // Arrange
            var failedRequest = "Test failed request";

            // Act
            FailedRequestsService.FailedRequests.Add(failedRequest);

            // Assert
            Assert.NotEmpty(FailedRequestsService.FailedRequests);
        }
    }
}
