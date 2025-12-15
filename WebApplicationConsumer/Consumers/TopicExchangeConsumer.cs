using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApplicationConsumer.Services;
using Messages;

namespace WebApplicationConsumer.Consumers
{
    public class TopicExchangeConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMqChannel _rabbitMqChannel;

        public TopicExchangeConsumer(IServiceProvider serviceProvider, IRabbitMqChannel rabbitMqChannel)
        {
            _rabbitMqChannel = rabbitMqChannel;
            _serviceProvider = serviceProvider;

            _channel = _rabbitMqChannel.GetChannel();

            RabbitMqTopologyInitializer.Configure(
                channel: _channel,
                exchange: Messages.Constants.EXCHANGE_TOPIC_DEMO,
                exchangeType: ExchangeType.Topic,
                queue: Messages.Constants.QUEUE_DEMO_TOPIC,
                Messages.Constants.TOPIC_ROUTING_KEY
                );

            _channel.BasicQos(0, 10, false);
        }

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                try
                {
                    var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                    var message = JsonConvert.DeserializeObject<MessageRabbit>(content);
                    if (message == null)
                        throw new JsonException("Mensagem inválida.");

                    NotifyUser(message);

                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (JsonException)
                {
                    // Erro definitivo → DLQ
                    _channel.BasicReject(eventArgs.DeliveryTag, requeue: false);
                }
                catch (Exception)
                {
                    // Erro transitório → retry
                    _channel.BasicNack(eventArgs.DeliveryTag, false, requeue: true);
                }
            };

            _channel.BasicConsume(
                queue: Messages.Constants.QUEUE_DEMO_TOPIC,
                autoAck: false, // NÃO remova a mensagem automaticamente quando entregá-la ao consumer.
                consumerTag: Messages.Constants.TOPIC_EXCHANGE_CONSUMER_NAME,
                consumer: consumer
                );

            return Task.CompletedTask;
        }

        private void NotifyUser(MessageRabbit message)
        {
            using var scope = _serviceProvider.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            notificationService.NotifyUser("TopicExchange", message.FromId, message.ToId, message.Content);
        }
    }
}
