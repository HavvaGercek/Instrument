using Instrument.Mail.Worker.Models;
using Instrument.Mail.Worker.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Mail;

namespace Instrument.Mail.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly IMailService _mailService;
        private const string QueueName = "alert.email";
        private const string ExchangeName = "alert.email.exchange";
        private const string RoutingKey = "alert.email.*";

        public Worker(ILogger<Worker> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };//change this

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(QueueName, false, false, false, null);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

                // handle the received message  
                HandleMessage(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(QueueName, false, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(string content)
        {
            var data = System.Text.Json.JsonSerializer.Deserialize<List<MessageModel>>(content,
                       new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (data != null)
            {
                foreach (var item in data)
                {
                    _mailService.Send(item.Email, "Instrument Price Alert", $"Instrument: {item.Symbol} - Price: {item.Price}");

                }
            }
            _logger.LogInformation($"consumer received {content}");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}