using Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace WebApplicationPublish.Publisher
{
    public class DirectExchangePublisher
    {
        public static void Publish(IModel channel, MessageRabbit message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(
                exchange: Messages.Constants.EXCHANGE_DIRECT_DEMO,
                routingKey: Messages.Constants.ROUTING_KEY_EXCHANGE_DIRECT,
                basicProperties: null,
                body: messageBytes
                );

        }
    }
}
