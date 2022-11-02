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
        public IActionResult SendToConsumer(ProccessedNews news)
        {
            count = new Random().Next(0, 100);
            _logger.LogInformation($"Received feedback from people regarding the curiosity'{news.message} which was indexed {news.index}'");
            return Ok();
        }
    };
 
}