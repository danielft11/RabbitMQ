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
            var header = new Dictionary<string, object>() {{ "account", "new" }};
            var properties = channel.CreateBasicProperties();
            properties.Headers = header;

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(
                exchange: Messages.Constants.EXCHANGE_HEADER_DEMO,
                routingKey: string.Empty,
                basicProperties: properties,
                body: messageBytes
            );

        }
    }
}