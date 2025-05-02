using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace EventsToCONNECTAPISampleTests
{
    public class IntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private HttpClient TestClient { get; set; }

        public IntegrationTests(TestWebApplicationFactory<Program> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            // Build test app with TestAuthHandler
            var webApplicationFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                });
            });

            TestClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task GetEventsShouldReturnEvents()
        {
            // Arrange
            var request = "/api/events";

            // Assert that our response is successful and it can be parsed into JSON
            var response = await TestClient.GetAsync(new Uri(request, UriKind.Relative)).ConfigureAwait(true);

            response.EnsureSuccessStatusCode();

            string jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            var jsonDocument = JsonDocument.Parse(jsonContent);

            // Assert that the JSON contains the "messageHeaders" object
            Assert.True(jsonDocument.RootElement.TryGetProperty("messageHeaders", out JsonElement messageHeaders));

            // Assert that messageHeaders contains messageType field
            Assert.True(messageHeaders.TryGetProperty("messageType", out JsonElement messageType));

            // Assert that messageType is correct
            Assert.Equal("Events", messageType.GetString());

            // Assert that the JSON contains the "messageBody" object
            Assert.True(jsonDocument.RootElement.TryGetProperty("messageBody", out JsonElement messageBody));

            // Assert that messageBody is an array
            Assert.Equal(JsonValueKind.Array, messageBody.ValueKind);

            // Iterate through the messageBody array and assert that each object contains an id field
            foreach (JsonElement item in messageBody.EnumerateArray())
            {
                Assert.True(item.TryGetProperty("id", out JsonElement id));
            }
        }
    }
}
