using System;
using System.Threading;
using Common;

namespace Sender
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var source = new CancellationTokenSource();

            var sendThread = new Thread(Send);
            sendThread.Start(source.Token);

            Console.WriteLine("please <enter> to stop");
            Console.ReadLine();

            source.Cancel(false);
            Console.WriteLine("please <enter> to exit");
            Console.ReadLine();
        }

        private static void OnMessageReceived(Message msg)
        {
            Thread.Sleep(msg.Complexity * 1000);
            Console.WriteLine(msg.ToString());
        }

        private static void Send(object startParam)
        {
            var message = new Message
            {
                Title = "title",
                Body = "Body"
            };

            using (var sender = new Producer<Message>("localhost", string.Empty, "add"))
            {
                var token = (CancellationToken)startParam;
                var random = new Random(DateTime.Now.Millisecond);
                while (!token.IsCancellationRequested)
                {
                    message.Complexity = random.Next(10);
                    sender.Send(message);
                }
            }
        }
    }
}
