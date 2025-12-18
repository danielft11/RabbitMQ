using Messages;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace WebApplicationConsumer
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly RabbitMqConfiguration _config;

        public RabbitMQConnection(IOptions<RabbitMqConfiguration> options) 
        {
            _config = options.Value;
        } 
        
        public IConnection GetConnection() 
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.UserName,
                Password = _config.Password
            };

            return factory.CreateConnection();
        }
    }

    public interface IRabbitMQConnection
    {
        IConnection GetConnection();
    }

}
