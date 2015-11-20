using System;

namespace RabbitMQSample
{
    public class Message
    {
        public string Body { get; set; }
        public int Counter { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return string.Format("Message[Title:'{0}',Body:'{1}',Counter:{2}]", Title, Body, Counter.ToString());
        }
    }
}
