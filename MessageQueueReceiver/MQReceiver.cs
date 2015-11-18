namespace MessageQueueReceiver
{
    using System;
    using System.Threading;
    using CloudServices.Common;
    using IronMQ;
    using IronMQ.Data;

    public class MQReceiver
    {
        public static void Main()
        {
            var client = new Client(GlobalConstatns.ProjectID, GlobalConstatns.Token);
            Queue queue = client.Queue("TelerikDemo");
            Console.WriteLine("Listening for new messages from IronMQ server:");
            
            while (true)
            {
                Message msg = queue.Get();
                if (msg != null)
                {
                    Console.WriteLine(msg.Body);
                    queue.DeleteMessage(msg);
                }

                Thread.Sleep(100);
            }
        }
    }
}
