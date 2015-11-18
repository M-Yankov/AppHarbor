namespace MessageQueueSender
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using CloudServices.Common;
    using IronMQ;

    public class MQSender
    {
        private const string MessageStringFormat = "{{{0}: {1}}}";
        private static string localMachineIpAddress; 

        public static void Main()
        {
            localMachineIpAddress = GetIpAddresFromLocalPc();

            var sender = new Client(GlobalConstatns.ProjectID, GlobalConstatns.Token);
            Queue queue = sender.Queue("TelerikDemo");

            Console.WriteLine("Send message:");
            while (true)
            {
                string message = Console.ReadLine();
                queue.Push(string.Format(MessageStringFormat, localMachineIpAddress, message));
            }
        }

        private static string GetIpAddresFromLocalPc()
        {
            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            return ipAddress.ToString();
        }
    }
}