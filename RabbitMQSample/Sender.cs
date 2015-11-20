using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQSample
{
    public class Sender<T>
    {
        private readonly string hostName;
        private readonly string queueName;

        public Sender(string serverName, string queueName)
        {
            hostName = serverName;
            this.queueName = queueName;
        }

        public void Send(T obj)
        {
            byte[] message = new Serializer<T>().Serialize(obj);
            Send(message);
        }

        private void Send(byte[] message)
        {
            var factory = new ConnectionFactory { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var chanel = connection.CreateModel())
            {
                ConfigureChanel(chanel);
                chanel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: null, body: message);
            }
        }

        private void ConfigureChanel(IModel chanel)
        {
            chanel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
    }

    public class Serializer<T>
    {
        public T Desearalize(byte[] bytes)
        {
            string jsonString = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public byte[] Serialize(T obj)
        {
            string jsonString = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(jsonString);
        }
    }
}
