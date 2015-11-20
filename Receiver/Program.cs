﻿using System;
using System.Threading;
using Common;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var receiver = new Receiver<Message>("localhost", "message_queue", OnMessageReceived))
            {
                receiver.Open();

                Console.WriteLine("please <enter> to exit");
                Console.ReadLine();
            }
        }
        private static void OnMessageReceived(Message msg)
        {
            Thread.Sleep(msg.Complexity * 1000);
            Console.WriteLine(msg.ToString());
        }
    }
}