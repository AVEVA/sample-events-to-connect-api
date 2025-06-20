﻿using System.Globalization;
using EventsToCONNECTAPISample.Models;

namespace EventsToCONNECTAPISample.Services
{
    public class EventsService
    {
        public List<PumpEvent> Events { get; }

        private System.Timers.Timer Timer { get; }

        private bool Pump1_On;
        private bool Pump2_On;

        public EventsService()
        {
            Events = new List<PumpEvent>();

            Timer = new(TimeSpan.FromMinutes(10));
            Timer.AutoReset = true;
            Timer.Elapsed += (s, e) => GetEvents();
            Timer.Start();

            Pump1_On = true;
            Pump2_On = false;

            GetEvents();
        }

        private void GetEvents()
        {
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddMinutes(1);

            PumpEvent pump1Event = new PumpEvent
            {
                Id = $"Pump1-{(Pump1_On ? "On" : "Off")}-{startTime:yyyyMMddHHmmss}-EventsToCONNECT",
                PumpStatus = Pump1_On ? "On" : "Off",
                StartTime = startTime,
                EndTime = endTime,
                Site = new Reference { Id = "Site1-EventsToCONNECT" },
                Asset = new Reference { Id = "Pump1-EventsToCONNECT" }
            };

            PumpEvent pump2Event = new PumpEvent
            {
                Id = $"Pump2-{(Pump2_On ? "On" : "Off")}-{startTime:yyyyMMddHHmmss}-EventsToCONNECT",
                PumpStatus = Pump2_On ? "On" : "Off",
                StartTime = startTime,
                EndTime = endTime,
                Site = new Reference { Id = "Site2-EventsToCONNECT" },
                Asset = new Reference { Id = "Pump2-EventsToCONNECT" }
            };

            Events.Add(pump1Event); 
            Events.Add(pump2Event);

            Pump1_On = !Pump1_On;
            Pump2_On = !Pump2_On;
        }
    }
}
