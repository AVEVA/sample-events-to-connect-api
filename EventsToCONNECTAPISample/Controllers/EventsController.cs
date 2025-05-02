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
        private string AssetTypeId { get; }
        private EventsService EventsService { get; }

        public EventsController(IConfiguration configuration, EventsService eventsService)
        {
            EventsService = eventsService;

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

            var assetTypeId = configuration.GetValue<string>("assetTypeId");

            if (string.IsNullOrEmpty(assetTypeId))
            {
                throw new MissingFieldException("Missing assetTypeId from appsettings.json!");
            }

            AssetTypeId = assetTypeId;
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

            List<PumpEvent> events;

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
                Id = "PumpStatus",
                Name = "PumpStatus",
                PropertyTypeCode = "String"
            };

            var pumpProperty = new PropertyDefinition
            {
                Id = "Pump",
                Name = "Pump",
                PropertyTypeCode = "Asset",
                PropertyTypeId = AssetTypeId
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

            typeDefinition.Properties.AddRange([sampleProperty, pumpProperty, siteProperty]);

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { typeDefinition }
            });
        }
    }
}
