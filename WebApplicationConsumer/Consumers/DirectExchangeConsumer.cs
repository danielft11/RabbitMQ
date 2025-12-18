using RabbitMQ.Client;
using System;

namespace WebApplicationConsumer.Consumer
{
    public class DirectExchangeConsumer : RabbitMQBaseConsumer
    {
        public DirectExchangeConsumer(IServiceProvider serviceProvider, IRabbitMQConnection rabbitMQConnection) : base(serviceProvider, rabbitMQConnection)
        {
            RabbitMqTopologyInitializer.Configure(
                channel: _channel,
                exchange: Messages.Constants.EXCHANGE_DIRECT_DEMO,
                exchangeType: ExchangeType.Direct,
                queue: Messages.Constants.QUEUE_DEMO_DIRECT,
                routingKey: Messages.Constants.ROUTING_KEY_EXCHANGE_DIRECT
                );
        }

        protected override string QueueName => Messages.Constants.QUEUE_DEMO_DIRECT;
        protected override string SourceName => Messages.Constants.EXCHANGE_DIRECT_DEMO;
        protected override string ConsumerTag => string.Empty;
    }

}
