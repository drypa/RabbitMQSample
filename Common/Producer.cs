using System;
using RabbitMQ.Client;

namespace Common
{
    public class Producer<TMessage>
        where TMessage : new()
    {
        private readonly string hostName;
        private readonly string queue;

        public Producer(string serverName, string queueName)
        {
            hostName = serverName;
            queue = queueName;
        }

        public void Send(TMessage obj)
        {
            byte[] message = new Serializer<TMessage>().Serialize(obj);
            Send(message);
        }

        private void ConfigureChanel(IModel chanel)
        {
            chanel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        private void Send(byte[] message)
        {
            var factory = new ConnectionFactory { HostName = hostName };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    ConfigureChanel(channel);
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = DeliveryMode.Persistent;
                    channel.BasicPublish(exchange: string.Empty, routingKey: queue, basicProperties: properties, body: message);
                }
            }
        }

        private static class DeliveryMode
        {
            public static byte NonPersistent
            {
                get { return 1; }
            }

            public static byte Persistent
            {
                get { return 2; }
            }
        }
    }
}
