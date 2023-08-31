using Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationPublish.Publishers
{
    public class HeaderExchangePublisher
    {
        public static void Publish(IModel channel, MessageRabbit message)
        {
            var ttl = new Dictionary<string, object>
            {
                { "x-message-ttl", 30000 }
            };

            channel.ExchangeDeclare(Messages.Constants.EXCHANGE_HEADER_DEMO, ExchangeType.Headers, arguments: ttl);
            channel.QueueDeclare(Messages.Constants.QUEUE_DEMO_HEADER, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var header = new Dictionary<string, object>() {{ "account", "new" }};
            
            channel.QueueBind(Messages.Constants.QUEUE_DEMO_HEADER, Messages.Constants.EXCHANGE_HEADER_DEMO, string.Empty, header);
            channel.BasicQos(0, 10, false);

            var messageSerialized = JsonConvert.SerializeObject(message);
            var messageBytes = Encoding.UTF8.GetBytes(messageSerialized);

            var properties = channel.CreateBasicProperties();
            properties.Headers = header;

            channel.BasicPublish(
                exchange: Messages.Constants.EXCHANGE_HEADER_DEMO,
                routingKey: string.Empty,
                basicProperties: properties,
                body: messageBytes
            );

        }
    }
}