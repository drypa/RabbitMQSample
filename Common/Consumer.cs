using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common
{
    public class Consumer<TMessage> : IDisposable
        where TMessage : new()
    {
        private readonly string HostName;
        private readonly Action<TMessage> messageReceivedAction;
        private readonly string queue;
        private IModel model;
        private IConnection connection;
        private EventingBasicConsumer consumer;

        public Consumer(string serverName, string queueName, Action<TMessage> onMessageReceived)
        {
            HostName = serverName;
            queue = queueName;
            messageReceivedAction = onMessageReceived;
        }

        public void Open()
        {
            var factory = new ConnectionFactory { HostName = HostName };
            connection = factory.CreateConnection();
            model = connection.CreateModel();
            ConfigureChanel(model);

            consumer = new EventingBasicConsumer(model);
            
            consumer.Received+= (sender, args) => ItemProcessing(args);
            model.BasicQos(0,2,true);
            model.BasicConsume(queue: queue, noAck: false, consumer: consumer);

        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
            if (model != null)
            {
                model.Dispose();
            }
        }

        private void ConfigureChanel(IModel chanel)
        {
            
            chanel.QueueDeclare(queue,  true, false, autoDelete: false, arguments: null);
        }

        private void ItemProcessing(BasicDeliverEventArgs e)
        {
            TMessage message = new Serializer<TMessage>().Desearalize(e.Body);
            messageReceivedAction(message);
            if (model != null)
            {
                model.BasicAck(e.DeliveryTag, false);
            }
        }
    }
}
