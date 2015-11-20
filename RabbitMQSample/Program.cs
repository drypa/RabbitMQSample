using System;
using System.Threading;

namespace RabbitMQSample
{
    class Program
    {
        static void Main(string[] args)
        {




            CancellationTokenSource source = new CancellationTokenSource();

            Thread sendThread = new Thread(Send);
            sendThread.Start(source.Token);

            //Даём набраться сообщениям в очереди
            Thread.Sleep(60000);
            using (Receiver<Message> receiver = new Receiver<Message>("localhost", "message_queue", (msg) => Console.WriteLine(msg.ToString())))
            {
                receiver.Open();

                Console.ReadLine();

                source.Cancel(false);

                Console.ReadLine();
            }

        }

        private static void Send(object startParam)
        {
            Message message = new Message
            {
                Title = "title",
                Body = "Body",
                Counter = 100
            };

            Sender<Message> sender = new Sender<Message>("localhost", "message_queue");
            CancellationToken token = (CancellationToken)startParam;
            while (!token.IsCancellationRequested)
            {
                sender.Send(message);
            }
        }


    }
}
