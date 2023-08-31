using Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationPublish.Publishers
{
    public class TopicExchangePublisher
    {
        public static void Publish(IModel channel, MessageRabbit message)
        {
            var ttl = new Dictionary<string, object>
            {
                { "x-message-ttl", 30000 }
            };

            channel.ExchangeDeclare(Messages.Constants.EXCHANGE_TOPIC_DEMO, ExchangeType.Topic, arguments: ttl);
            channel.QueueDeclare(Messages.Constants.QUEUE_DEMO_TOPIC, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind(Messages.Constants.QUEUE_DEMO_TOPIC, Messages.Constants.EXCHANGE_TOPIC_DEMO, "account.*");
            channel.BasicQos(0, 10, false);

            var messageSerialized = JsonConvert.SerializeObject(message);
            var messageBytes = Encoding.UTF8.GetBytes(messageSerialized);

            channel.BasicPublish(
                exchange: Messages.Constants.EXCHANGE_TOPIC_DEMO,
                routingKey: "account.update",
                basicProperties: null,
                body: messageBytes
                );

        }
    }
}
