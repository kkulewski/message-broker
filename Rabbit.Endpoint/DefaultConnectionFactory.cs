using RabbitMQ.Client;

namespace Rabbit.Endpoint
{
    public static class FactoryProvider
    {
        public static IConnectionFactory Instance { get; }

        static FactoryProvider()
        {
            Instance = new ConnectionFactory
            {
                HostName = BrokerEndpoint.HostName,
                UserName = BrokerEndpoint.UserName,
                VirtualHost = BrokerEndpoint.VirtualHost,
                Password = BrokerEndpoint.Password
            };
        }
    }
}
