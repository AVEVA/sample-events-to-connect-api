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
        private string AuthorizationTagId { get; }
        private EventsService EventsService { get; }

        public EventsController(IConfiguration configuration, EventsService eventsService)
        {
            EventTypeId = configuration.GetValue("eventTypeId", "EventTypeId-EventsToCONNECT")!;
            ReferenceDataTypeId = configuration.GetValue("referenceDataTypeId", "ReferenceDataTypeId-EventsToCONNECT")!;
            AuthorizationTagId = configuration.GetValue("authorizationTagId", "AuthorizationTagId-EventsToCONNECT")!;

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

            var statusProperty = new PropertyDefinition
            {
                Id = "PumpStatus",
                Name = "PumpStatus",
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
                Name = EventTypeId,
                DefaultAuthorizationTag = AuthorizationTagId
            };

            typeDefinition.Properties.AddRange([statusProperty, siteProperty]);

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { typeDefinition }
            });
        }
    }
}
