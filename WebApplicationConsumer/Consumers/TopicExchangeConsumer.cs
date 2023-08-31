using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApplicationConsumer.Services;

namespace WebApplicationConsumer.Consumers
{
    public class TopicExchangeConsumer : BackgroundService
    {
        private readonly RabbitMqConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public TopicExchangeConsumer(IOptions<RabbitMqConfiguration> options, IServiceProvider serviceProvider)
        {
            _config = _config = options.Value;
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.UserName,
                Password = _config.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: Messages.Constants.QUEUE_DEMO_TOPIC,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<MessageRabbit>(contentString);

                NotifyUser(message);

                // informa ao RabbitMQ que a entrega da mensagem foi realizada
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            // inicia o consumo da mensagem
            _channel.BasicConsume(Messages.Constants.QUEUE_DEMO_TOPIC, false, consumer);

            return Task.CompletedTask;
        }

        private void NotifyUser(MessageRabbit message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                notificationService.NotifyUser(message.FromId, message.ToId, message.Content);
            }
        }
    }
}
