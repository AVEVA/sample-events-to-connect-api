using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace EventsToCONNECTAPISampleTests
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();

                string configFile = File.Exists(Path.Combine(context.HostingEnvironment.ContentRootPath, "appsettings.json")) 
                    ? "appsettings.json" 
                    : "appsettings.placeholder.json";

                config.AddJsonFile(configFile, optional: false, reloadOnChange: true);
            });
        }
    }

}
