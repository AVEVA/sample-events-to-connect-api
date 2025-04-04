using EventsToCONNECTAPISample.Models;
using EventsToCONNECTAPISample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsToCONNECTAPISample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private string EventTypeId { get; }
        private string ReferenceDataTypeId { get; }
        private EventsService EventsService { get; }

        public EventsController(IConfiguration configuration, EventsService eventsService)
        {
            var eventTypeId = configuration.GetValue<string>("eventTypeId");

            if (string.IsNullOrEmpty(eventTypeId))
            {
                throw new MissingFieldException("Missing eventTypeId from appsettings.json!");
            }

            EventTypeId = eventTypeId;

            var referenceDataTypeId = configuration.GetValue<string>("referenceDataTypeId");

            if (string.IsNullOrEmpty(referenceDataTypeId))
            {
                throw new MissingFieldException("Missing referenceDataTypeId from appsettings.json!");
            }

            ReferenceDataTypeId = referenceDataTypeId;

            EventsService = eventsService;
        }

        // GET: api/Events
        [HttpGet]
        public IActionResult Get(string site="")
        {
            var header = new MessageHeader
            {
                MessageType = "Events",
                Action = "Create",
                Format = "Json",
                TypeId = EventTypeId
            };

            List<SampleEvent> events;

            if (!string.IsNullOrEmpty(site))
            {
                events = EventsService.Events.Where(e => e.Site?.Id?.Contains(site) ?? false).ToList();
            }
            else
            {
                events = EventsService.Events;
            }

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = events
            });
        }

        // GET: api/Events/Type
        [HttpGet("Type")]
        public IActionResult GetEventType()
        {
            var header = new MessageHeader
            {
                MessageType = "EventTypes",
                Action = "Create",
                Format = "Json",
                TypeId = EventTypeId
            };

            var sampleProperty = new PropertyDefinition
            {
                Id = "Sample",
                Name = "Sample",
                PropertyTypeCode = "String"
            };

            var siteProperty = new PropertyDefinition
            {
                Id = "Site",
                Name = "Site",
                PropertyTypeCode = "ReferenceData",
                PropertyTypeId = ReferenceDataTypeId
            };

            var typeDefinition = new TypeDefinition
            {
                Id = EventTypeId,
                Name = EventTypeId
            };

            typeDefinition.Properties.AddRange([sampleProperty, siteProperty]);

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { typeDefinition }
            });
        }
    }
}
