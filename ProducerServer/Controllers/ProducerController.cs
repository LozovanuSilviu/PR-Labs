using Microsoft.AspNetCore.Mvc;
using ProducerServer.Models;

namespace ProducerServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;
        private int count;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }

        [HttpPost("send/to/producer")]
        public IActionResult SendToConsumer(News news)
        {
            count = new Random().Next(0, 100);
            _logger.LogInformation($"Received feedback from people regarding the curiosity'{news.Message}'");
            return Ok();
        }
    };
 
}