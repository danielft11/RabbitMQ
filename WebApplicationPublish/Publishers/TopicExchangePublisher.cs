using Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace WebApplicationPublish.Publishers
{
    public class TopicExchangePublisher
    {
        public static void Publish(IModel channel, MessageRabbit message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(
                exchange: Messages.Constants.EXCHANGE_TOPIC_DEMO,
                routingKey: "account.update",
                basicProperties: null,
                body: messageBytes
                );
        }

    }
}
