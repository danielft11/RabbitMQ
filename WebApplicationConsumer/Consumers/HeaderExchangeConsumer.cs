using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace WebApplicationConsumer.Consumer
{
    public class HeaderExchangeConsumer : RabbitMQBaseConsumer
    {
        public HeaderExchangeConsumer(IServiceProvider serviceProvider, IRabbitMQConnection rabbitMQConnection) : base(serviceProvider, rabbitMQConnection)
        {
            var header = new Dictionary<string, object>() { { "account", "new" } };

            RabbitMqTopologyInitializer.Configure(
                channel: _channel,
                exchange: Messages.Constants.EXCHANGE_HEADER_DEMO,
                exchangeType: ExchangeType.Headers,
                queue: Messages.Constants.QUEUE_DEMO_HEADER,
                routingKey: string.Empty,
                arguments: header
                );

            _channel.BasicQos(0, 10, false);

        }

        protected override string QueueName => Messages.Constants.QUEUE_DEMO_HEADER;
        protected override string SourceName => Messages.Constants.EXCHANGE_HEADER_DEMO;
        protected override string ConsumerTag => string.Empty;
    }

}