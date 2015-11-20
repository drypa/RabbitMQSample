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

            Sender<Message> sender = new Sender<Message>("localhost", "message_queue");
            sender.Send(message);

            using (Receiver<Message> receiver = new Receiver<Message>("localhost", "message_queue", (msg) => Console.WriteLine(msg.ToString())))
            {
                receiver.Open();
                Console.ReadLine();
            }


            Console.ReadLine();
        }

    }
}
