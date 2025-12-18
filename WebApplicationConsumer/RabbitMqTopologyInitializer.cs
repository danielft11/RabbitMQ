using RabbitMQ.Client;
using System.Collections.Generic;

namespace WebApplicationConsumer
{
    public static class RabbitMqTopologyInitializer
    {
        public static void Configure(IModel channel, string exchange, string exchangeType, string queue, string routingKey, IDictionary<string, object> arguments = null) 
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

            if (arguments != null)
            {
                foreach (var arg in arguments)
                {
                    argsDictionary.Add(arg.Key, arg.Value);
                }
            }

            channel.ExchangeDeclare(exchange, exchangeType, durable: true);

            channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: argsDictionary // Define a DLX e DLQ para essa fila
            );

            //Liga a fila à exchange e define quais mensagens essa fila vai recebe
            channel.QueueBind(
                queue: queue,
                exchange: exchange,
                routingKey: routingKey
            );

        }
    }
}
