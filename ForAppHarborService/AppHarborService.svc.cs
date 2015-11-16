namespace ForAppHarborService
{
    using System;
    using System.Threading;
    using CloudServices.Common;
    using IronMQ;
    using IronMQ.Data;
    using System.Net;
    using System.Linq;
    using System.Net.Sockets;

    public class AppHarborService : IAppHarborService
    {
        public string GetMessage()
        {
            var client = new Client(GlobalConstatns.ProjectID, GlobalConstatns.Token);
            Queue queue = client.Queue("TelerikDemo");
            //Console.WriteLine("Listening for new messages from IronMQ server:");
            Message message = queue.Get();
            string result = "No new messages."; 
            if (message != null)
            {
                result = message.Body;
                queue.DeleteMessage(message);
            }

            return result;

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

        public void SendMessage(string text)
        {
            string localMachineIpAddress = this.GetIpAddresFromLocalPc();

            var sender = new Client(GlobalConstatns.ProjectID, GlobalConstatns.Token);
            Queue queue = sender.Queue("TelerikDemo");
            queue.Push(string.Format("{{{0}: {1}}}", localMachineIpAddress, text));
            //Console.WriteLine("Send message:");
            //while (true)
            //{
            //    string message = Console.ReadLine();
               
            //}
        }

        private string GetIpAddresFromLocalPc()
        {
            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            return ipAddress.ToString();
        }
    }
}
