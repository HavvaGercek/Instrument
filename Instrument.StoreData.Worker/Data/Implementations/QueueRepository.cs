using Instrument.StoreData.Worker.Data.Interfaces;
using Instrument.StoreData.Worker.Models.Domain;
using System.Text;
using RabbitMQ.Client;

namespace Instrument.StoreData.Worker.Data.Implementations
{
    public class QueueRepository : IQueueRepository
    {
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "alert.email";
        private const string ExchangeName = "alert.email.exchange";
        private const string RoutingKey = "alert.email.*";
        public QueueRepository()
        {
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
        }

        public async Task AddAsync(QueueAddModel model)
        {
            var body = Encoding.UTF8.GetBytes(model.Body);

            _channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey, basicProperties: null, body: body);

        }
    }
}
