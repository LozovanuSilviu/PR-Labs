using Microsoft.AspNetCore.Mvc;
using ProducerServer.Models;

namespace ProducerServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }

        [HttpPost("send/to/producer")]
        public IActionResult SendToConsumer(Letter letter)
        {
            _logger.LogInformation($"Received back the letter with message '{letter.Message}'");
            return Ok();
        }
    };
 
}