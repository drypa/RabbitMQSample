using System;

namespace RabbitMQSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Message message = new Message
            {
                Title = "title",
                Body = "Body",
                Counter = 100
            };

            Sender<Message> sender = new Sender<Message>("localhost","message_queue");
            sender.Send(message);
        }

    }

    public class Message
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int Counter { get; set; }
    }
}
