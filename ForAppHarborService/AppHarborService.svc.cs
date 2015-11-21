namespace ForAppHarborService
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.ServiceModel.Web;
    using System.Text;
    using CloudServices.Common;
    using CloudServices.Common.ResponseTemplates;
    using IronMQ;
    using IronMQ.Data;

    public class AppHarborService : IAppHarborService
    {
        public Stream GetMessage()
        {
            WebOperationContext context = WebOperationContext.Current;
            /* if (context.IncomingRequest.ContentType.Contains("/xml"))// /josn
             {
                WebOperationContext.Current.OutgoingResponse.Format = WebMessageFormat.Json;
             }*/

            //Queue queue = ClientProvider.GetClient().Queue("TelerikDemo").Get(20);
            IList<Message> messages = ClientProvider.GetClient().Queue("TelerikDemo").Get(20);

            var result = new StringBuilder();
            if (messages.Count != 0)
            {
                string formated = string.Format("<li>{0}</li>", string.Join("</li><li>", messages.Select(x => x.Body)));
                result.AppendLine(formated);
            }
            else
            {
                result.AppendLine("<li>No new messages</li>");
            }

            if (messages.Count > 10)
            {
                int count = messages.Count;
                int index = 0;
                while (count > 10)
                {
                    ClientProvider.GetClient().Queue("TelerikDemo").DeleteMessage(messages[index]);
                    index++;
                    count--;
                }
            }

            string content = string.Format(GetMessagesTemplate.HtmlTemplate, result.ToString());
            byte[] resultBytes = Encoding.UTF8.GetBytes(content);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return new MemoryStream(resultBytes);
        }

        public Stream SendMessage()
        {
            byte[] resultBytes = Encoding.UTF8.GetBytes(GetMessagesTemplate.HtmlSendTemplate);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return new MemoryStream(resultBytes);
        }

        public void SendRealMessageMessage(ComplexStiupidShit data)
        {
            string localMachineIpAddress = this.GetIpAddresFromLocalPc();

            var sender = new Client(GlobalConstatns.ProjectID, GlobalConstatns.Token);
            Queue queue = sender.Queue("TelerikDemo");
            queue.Push(string.Format("{{{0}: {1}}}", localMachineIpAddress, data.Text));
        }

        private string GetIpAddresFromLocalPc()
        {
            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            return ipAddress.ToString();
        }
    }
}
