using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQSample
{
    public class Receiver<TMessage> : IDisposable
        where TMessage : new()
    {
        private readonly string HostName;
        private readonly Action<TMessage> messageReceivedAction;
        private readonly string queue;
        private IModel channel;
        private IConnection connection;
        private EventingBasicConsumer consumer;

        public Receiver(string serverName, string queueName, Action<TMessage> onMessageReceived)
        {
            HostName = serverName;
            queue = queueName;
            messageReceivedAction = onMessageReceived;
        }

        public void Open()
        {
            var factory = new ConnectionFactory { HostName = HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            ConfigureChanel(channel);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += consumer_Received;
            channel.BasicConsume(queue: queue,
                                 noAck: true,
                                 consumer: consumer);
        }

        public void Dispose()
        {
            if (consumer != null)
            {
                consumer.Received -= consumer_Received;
            }
            if (connection != null)
            {
                connection.Dispose();
            }
            if (channel != null)
            {
                channel.Dispose();
            }
        }

        private void ConfigureChanel(IModel chanel)
        {
            chanel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            TMessage message = new Serializer<TMessage>().Desearalize(e.Body);
            messageReceivedAction(message);
        }
    }
}
