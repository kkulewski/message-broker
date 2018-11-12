using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Rabbit.Producer
{
    public class Worker
    {
        private readonly Random _random;
        private readonly IConnectionFactory _connectionFactory;

        public Worker(int id, IConnectionFactory connectionFactory)
        {
            Id = id;
            _random = new Random();
            _connectionFactory = connectionFactory;
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

                string message = $"[{DateTime.Now}] Number {delay} from worker {Id}!";
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
