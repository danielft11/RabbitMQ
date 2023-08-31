using Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WebApplicationPublish.Publisher;
using WebApplicationPublish.Publishers;

namespace WebApplicationPublish.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        private readonly RabbitMqConfiguration _config;

        public MessagesController(IOptions<RabbitMqConfiguration> options)
        {
            _config = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.UserName, 
                Password = _config.Password
            };
        }

        [HttpPost]
        [Route("by-direct-exchange")]
        public IActionResult PostMessageDirectExchange([FromBody] MessageRabbit message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            DirectExchangePublisher.Publish(channel, message);
            
            return Accepted();
        }

        [HttpPost]
        [Route("by-topic-exchange")]
        public IActionResult PostMessageByTopicExchange([FromBody] MessageRabbit message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            TopicExchangePublisher.Publish(channel, message);

            return Accepted();
        }
        
        [HttpPost]
        [Route("by-header-exchange")]
        public IActionResult PostMessageByHeaderExchange([FromBody] MessageRabbit message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            HeaderExchangePublisher.Publish(channel, message);

            return Accepted();
        }
    }
}
