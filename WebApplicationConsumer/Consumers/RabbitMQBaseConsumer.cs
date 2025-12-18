using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;
using System.Threading;
using WebApplicationConsumer.Services;
using System.Text;
using Newtonsoft.Json;

namespace WebApplicationConsumer.Consumer
{
    public abstract class RabbitMQBaseConsumer : BackgroundService
    {
        protected readonly IModel _channel;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IRabbitMQConnection _rabbitMQConnection;

        protected abstract string QueueName { get; }
        protected abstract string ConsumerTag { get; }
        protected abstract string SourceName { get; }

        protected RabbitMQBaseConsumer(IServiceProvider serviceProvider, IRabbitMQConnection rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _serviceProvider = serviceProvider;

            var connection = _rabbitMQConnection.GetConnection();
            _channel = connection.CreateModel();

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

                    NotifyUser(message, SourceName);

                    // informa ao RabbitMQ que a entrega da mensagem foi realizada
                    _channel.BasicAck(eventArgs.DeliveryTag, false);

                }
                catch (JsonException)
                {
                    // Erro definitivo → requeue = false -> mensagem é descartada. Vai para o DLX e em seguida para a DLQ
                    _channel.BasicReject(eventArgs.DeliveryTag, requeue: false);
                }
                catch (Exception)
                {
                    // Erro transitório → requeue = true -> mensagem volta para a fila
                    _channel.BasicNack(eventArgs.DeliveryTag, false, requeue: true);
                }
            };

            // inicia o consumo da mensagem
            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false, // NÃO remova a mensagem automaticamente quando entregá-la ao consumer.
                consumerTag: ConsumerTag,
                consumer: consumer
                );

            return Task.CompletedTask;
        }

        protected void NotifyUser(MessageRabbit message, string exchange)
        {
            using var scope = _serviceProvider.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            notificationService.NotifyUser(exchange, message.FromId, message.ToId, message.Content);
        }

    }
}
