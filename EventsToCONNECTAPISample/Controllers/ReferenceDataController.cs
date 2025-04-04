using EventsToCONNECTAPISample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsToCONNECTAPISample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReferenceDataController : ControllerBase
    {
        private string ReferenceDataTypeId { get; }

        public ReferenceDataController(IConfiguration configuration)
        {
            var referenceDataTypeId = configuration.GetValue<string>("referenceDataTypeId");

            if (string.IsNullOrEmpty(referenceDataTypeId))
            {
                throw new MissingFieldException("Missing referenceDataTypeId from appsettings.json!");
            }

            ReferenceDataTypeId = referenceDataTypeId;
        }

        // GET: api/ReferenceData
        [HttpGet]
        public IActionResult Get()
        {
            var header = new MessageHeader
            {
                MessageType = "ReferenceData",
                Action = "Create",
                Format = "Json",
                TypeId = ReferenceDataTypeId
            };

            var site1 = new SiteReferenceData
            {
                Id = "Site1",
                Name = "Site1",
                Address = "164 S Industry Blvd.",
                NumEmployees = "236",
            };

            var site2 = new SiteReferenceData
            {
                Id = "Site2",
                Name = "Site2",
                Address = "570 W Evans St.",
                NumEmployees = "1218",
            };

            var site3 = new SiteReferenceData
            {
                Id = "Site3",
                Name = "Site3",
                Address = "308 N Arroyo Ln.",
                NumEmployees = "737",
            };

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { site1, site2, site3 }
            });
        }

        // GET: api/ReferenceData/Type
        [HttpGet("Type")]
        public IActionResult GetReferenceDataType()
        {
            var header = new MessageHeader
            {
                MessageType = "ReferenceDataTypes",
                Action = "Create",
                Format = "Json",
                TypeId = ReferenceDataTypeId
            };

            var addressProperty = new PropertyDefinition
            {
                Id = "Address",
                Name = "Address",
                PropertyTypeCode = "String"
            };

            var numEmployeesProperty = new PropertyDefinition
            {
                Id = "NumEmployees",
                Name = "NumEmployees",
                PropertyTypeCode = "String"
            };

            var typeDefinition = new TypeDefinition
            {
                Id = ReferenceDataTypeId,
                Name = ReferenceDataTypeId
            };

            typeDefinition.Properties.AddRange([addressProperty, numEmployeesProperty]);

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { typeDefinition }
            });
        }
    }
}
