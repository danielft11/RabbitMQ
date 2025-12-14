using RabbitMQ.Client;
using System.Collections.Generic;

namespace WebApplicationConsumer
{
    public static class RabbitMqTopologyInitializer
    {
        public static void Configure(IModel channel) 
        {
            // DLX
            channel.ExchangeDeclare("demo.dlx", ExchangeType.Direct, durable: true);
            channel.QueueDeclare("demo.dlq", durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind("demo.dlq", "demo.dlx", "demo.dlq");

            // Queue principal com DLX
            var argsDictionary = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "demo.dlx" },
                { "x-dead-letter-routing-key", "demo.dlq" }
            };

            channel.ExchangeDeclare(Messages.Constants.EXCHANGE_TOPIC_DEMO, ExchangeType.Topic, durable: true);

            channel.QueueDeclare(
                queue: Messages.Constants.QUEUE_DEMO_TOPIC,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: argsDictionary // Define a DLX e DLQ para essa fila
            );

            //Liga a fila queue_demo_topic à exchange exchange_topic_demo e define quais mensagens essa fila vai recebe
            channel.QueueBind(
                queue: Messages.Constants.QUEUE_DEMO_TOPIC,
                exchange: Messages.Constants.EXCHANGE_TOPIC_DEMO,
                routingKey: Messages.Constants.TOPIC_ROUTING_KEY
            );

        }
    }
}
