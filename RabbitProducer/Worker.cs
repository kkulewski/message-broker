using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace RabbitProducer
{
    public class Worker
    {
        private readonly Random _random;
        private readonly ConnectionFactory _connectionFactory;

        public Worker(int id)
        {
            Id = id;
            _random = new Random();
            _connectionFactory = new ConnectionFactory
            {
                HostName = BrokerEndpoint.HostName,
                UserName = BrokerEndpoint.UserName,
                VirtualHost = BrokerEndpoint.VirtualHost,
                Password = BrokerEndpoint.Password
            };
        }

        public int Id { get; }

        public void SendMessage()
        {
            var delay = _random.Next(500, 10000);
            Thread.Sleep(delay);

            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = $"[{DateTime.Now}] Number {delay} from worker {this.Id}!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);
            }

            Console.WriteLine($"[{Id}]: Message sent!");
        }
    }
}
