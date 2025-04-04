using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsToCONNECTAPISample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        // GET: api/HealthCheck
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            await Task.Yield();

            return Ok();
        }
    }
}
