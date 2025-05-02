using EventsToCONNECTAPISample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsToCONNECTAPISample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationTagsController : ControllerBase
    {
        private string AuthorizationTagId { get; }

        public AuthorizationTagsController(IConfiguration configuration)
        {
            AuthorizationTagId = configuration.GetValue("authorizationTagId", "AuthorizationTagId-EventsToCONNECT")!;
        }

        // GET: api/AuthorizationTags
        [HttpGet]
        public IActionResult Get()
        {
            var header = new MessageHeader
            {
                MessageType = "AuthorizationTags",
                Action = "Create",
                Format = "Json",
                TypeId = AuthorizationTagId
            };

            var tag = new AuthorizationTag
            {
                Id = AuthorizationTagId,
                Description = "Events to CONNECT Sample Authorization tag",
            };

            return Ok(new
            {
                MessageHeaders = header,
                MessageBody = new[] { tag }
            });
        }
    }
}
