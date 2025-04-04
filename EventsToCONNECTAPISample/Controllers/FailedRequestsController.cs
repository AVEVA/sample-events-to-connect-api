using EventsToCONNECTAPISample.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsToCONNECTAPISample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FailedRequestsController : ControllerBase
    {
        private FailedRequestsService FailedRequestsService { get; }

        public FailedRequestsController(FailedRequestsService failedRequestsService)
        {
            FailedRequestsService = failedRequestsService;
        }

        // GET: api/FailedRequests
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(FailedRequestsService.FailedRequests);
        }

        // POST: api/FailedRequests
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var streamReader = new StreamReader(Request.Body);

            var requestString = await streamReader.ReadToEndAsync().ConfigureAwait(false);

            FailedRequestsService.FailedRequests.Add(requestString);

            return Ok();
        }
    }
}
