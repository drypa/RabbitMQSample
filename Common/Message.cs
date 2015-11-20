using System;

namespace Common
{
    public class Message
    {
        public string Body { get; set; }
        public int Complexity { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return string.Format("Message[Title:'{0}',Body:'{1}',Complexity:{2}]", Title, Body, Complexity.ToString());
        }
    }
}
