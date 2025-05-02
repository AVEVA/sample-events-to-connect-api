using EventsToCONNECTAPISample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsToCONNECTAPISample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private string AssetTypeId { get; }

        public AssetsController(IConfiguration configuration)
        {
            var assetTypeId = configuration.GetValue<string>("assetTypeId");

            if (string.IsNullOrEmpty(assetTypeId))
            {
                throw new MissingFieldException("Missing assetTypeId from appsettings.json!");
            }

            AssetTypeId = assetTypeId;
        }

        // GET: api/Assets
        [HttpGet]
        public IActionResult Get()
        {
            var header = new MessageHeader
            {
                MessageType = "Assets",
                Action = "Create",
                Format = "Json",
                TypeId = AssetTypeId
            };

            var pump1 = new PumpAsset
            {
                Id = "Pump1-EventsToCONNECT",
                Name = "Pump1",
                Model = "SB129",
                YearBuilt = "1999",
            };

            var pump2 = new PumpAsset
            {
                Id = "Pump2-EventsToCONNECT",
                Name = "Pump2",
                Model = "XJ220",
                YearBuilt = "1994",
            };

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { pump1, pump2 }
            });
        }

        // GET: api/Assets/Type
        [HttpGet("Type")]
        public IActionResult GetAssetType()
        {
            var header = new MessageHeader
            {
                MessageType = "AssetTypes",
                Action = "Create",
                Format = "Json",
                TypeId = AssetTypeId
            };

            var modelProperty = new PropertyDefinition
            {
                Id = "Model",
                Name = "Model",
                PropertyTypeCode = "String"
            };

            var yearBuiltProperty = new PropertyDefinition
            {
                Id = "YearBuilt",
                Name = "Year Built",
                PropertyTypeCode = "String"
            };

            var typeDefinition = new TypeDefinition
            {
                Id = AssetTypeId,
                Name = AssetTypeId
            };

            typeDefinition.Properties.AddRange([modelProperty, yearBuiltProperty]);

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { typeDefinition }
            });
        }
    }
}
