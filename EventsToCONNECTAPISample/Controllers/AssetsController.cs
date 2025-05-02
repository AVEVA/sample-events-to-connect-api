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

            var pump1 = new Asset
            {
                Id = "Pump1-EventsToCONNECT",
                Name = "Pump1",
                Description = "Events to CONNECT Sample Asset"
            };

            pump1.Metadata.AddRange([
                new MetadataItem { 
                    Id = "Model", 
                    Value = "SB120", 
                    SdsTypeCode = "String" 
                }, 
                new MetadataItem { 
                    Id = "YearBuilt", 
                    Value = "1999", 
                    SdsTypeCode = "String" 
                }
            ]);

            var pump2 = new Asset
            {
                Id = "Pump2-EventsToCONNECT",
                Name = "Pump2",
                Description = "Events to CONNECT Sample Asset"
            };

            pump2.Metadata.AddRange([
                new MetadataItem { 
                    Id = "Model", 
                    Value = "XJ220", 
                    SdsTypeCode = "String" }, 
                new MetadataItem { 
                    Id = "YearBuilt", 
                    Value = "1994", 
                    SdsTypeCode = "String" }
            ]);

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

            var modelMetadata = new MetadataDefinition
            {
                Id = "Model",
                Name = "Model",
                SdsTypeCode = SdsTypeCodes.SdsString
            };

            var yearBuiltMetadata = new MetadataDefinition
            {
                Id = "YearBuilt",
                Name = "Year Built",
                SdsTypeCode = SdsTypeCodes.SdsString
            };

            var assetDefinition = new AssetDefinition
            {
                Id = AssetTypeId,
                Name = AssetTypeId,
                Description = "Events to CONNECT Sample Asset Type"
            };

            assetDefinition.Metadata.AddRange([modelMetadata, yearBuiltMetadata]);

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { assetDefinition }
            });
        }
    }
}
