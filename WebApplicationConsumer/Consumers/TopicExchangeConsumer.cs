using RabbitMQ.Client;
using System;

namespace WebApplicationConsumer.Consumer
{
    public class TopicExchangeConsumer : RabbitMQBaseConsumer
    {
        public TopicExchangeConsumer(IServiceProvider serviceProvider, IRabbitMQConnection rabbitMQConnection) : base(serviceProvider, rabbitMQConnection)
        {
            RabbitMqTopologyInitializer.Configure(
                channel: _channel,
                exchange: Messages.Constants.EXCHANGE_TOPIC_DEMO,
                exchangeType: ExchangeType.Topic,
                queue: Messages.Constants.QUEUE_DEMO_TOPIC,
                routingKey: Messages.Constants.TOPIC_ROUTING_KEY
                );

            _channel.BasicQos(0, 10, false);
        }

        protected override string QueueName => Messages.Constants.QUEUE_DEMO_TOPIC;
        protected override string SourceName => Messages.Constants.EXCHANGE_TOPIC_DEMO;
        protected override string ConsumerTag => Messages.Constants.TOPIC_EXCHANGE_CONSUMER_NAME;
    }

}
