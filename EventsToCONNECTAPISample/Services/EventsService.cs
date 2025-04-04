using System.Globalization;
using EventsToCONNECTAPISample.Models;

namespace EventsToCONNECTAPISample.Services
{
    public class EventsService
    {
        public List<SampleEvent> Events { get; }

        private System.Timers.Timer Timer { get; }

        public EventsService()
        {
            Events = new List<SampleEvent>();

            GetEvents();

            Timer = new(TimeSpan.FromHours(1));
            Timer.AutoReset = true;
            Timer.Elapsed += (s, e) => GetEvents();
            Timer.Start();
        }

        private void GetEvents()
        {
            Events.Clear();

            var currentTime = DateTime.Now;

            for (int i = 1; i <= 3; i++)
            {
                string siteId = "Site" + i;

                Events.Add(new SampleEvent
                {
                    Id = $"{siteId}_{currentTime:yyyyMMdd-HHmmss}",
                    StartTime = currentTime.AddHours(-1),
                    EndTime = currentTime,
                    Sample = (Random.Shared.NextDouble() * 100).ToString(new CultureInfo("en-US")),
                    Site = new SiteReference { Id = siteId }
                });
            }
        }
    }
}
