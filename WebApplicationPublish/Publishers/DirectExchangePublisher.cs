using Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationPublish.Publisher
{
    public class DirectExchangePublisher
    {
        public static void Publish(IModel channel, MessageRabbit message)
        {
            var ttl = new Dictionary<string, object>
            {
                { "x-message-ttl", 30000 }
            };

            channel.ExchangeDeclare(Messages.Constants.EXCHANGE_DIRECT_DEMO, ExchangeType.Direct, arguments: ttl);
            channel.QueueDeclare(Messages.Constants.QUEUE_DEMO_DIRECT, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind(Messages.Constants.QUEUE_DEMO_DIRECT, Messages.Constants.EXCHANGE_DIRECT_DEMO, Messages.Constants.ROUTING_KEY_EXCHANGE_DIRECT);
            channel.BasicQos(0, 10, false);

            var messageSerialized = JsonConvert.SerializeObject(message);
            var messageBytes = Encoding.UTF8.GetBytes(messageSerialized);

            channel.BasicPublish(
                exchange: Messages.Constants.EXCHANGE_DIRECT_DEMO,
                routingKey: Messages.Constants.ROUTING_KEY_EXCHANGE_DIRECT,
                basicProperties: null,
                body: messageBytes
                );

        }
    }
}
