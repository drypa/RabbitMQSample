using System;
using RabbitMQ.Client;

namespace RabbitMQSample
{
    public class Sender<TMessage> where TMessage : new()
    {
        private readonly string hostName;
        private readonly string queue;

        public Sender(string serverName, string queueName)
        {
            hostName = serverName;
            queue = queueName;
        }

        public void Send(TMessage obj)
        {
            byte[] message = new Serializer<TMessage>().Serialize(obj);
            Send(message);
        }

        private void Send(byte[] message)
        {
            var factory = new ConnectionFactory { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var chanel = connection.CreateModel())
            {
                ConfigureChanel(chanel);
                chanel.BasicPublish(exchange: string.Empty, routingKey: queue, basicProperties: null, body: message);
            }
        }

        private void ConfigureChanel(IModel chanel)
        {
            chanel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
    }
}
