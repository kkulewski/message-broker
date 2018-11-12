using System;
using System.Text;
using Rabbit.Endpoint;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.Consumer
{
    class Program
    {
        public static void Main()
        {
            using (var connection = FactoryProvider.Instance.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                };
                channel.BasicConsume(
                    queue: "hello",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
