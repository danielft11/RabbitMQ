using Messages;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace WebApplicationConsumer
{
    public class RabbitMqChannel : IRabbitMqChannel
    {
        private readonly RabbitMqConfiguration _config;
        private readonly IConnection _connection;
        private readonly IConnectionFactory factory;

        public RabbitMqChannel(IOptions<RabbitMqConfiguration> options) 
        {
            _config = options.Value;
            
            factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.UserName,
                Password = _config.Password
            };

            _connection = factory.CreateConnection();
        } 
        
        public IModel GetChannel() 
        {
            return _connection.CreateModel();
        }
    }

    public interface IRabbitMqChannel
    {
        IModel GetChannel();
    }

}
