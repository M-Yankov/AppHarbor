namespace ForAppHarborService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Sockets;
    using System.ServiceModel.Web;
    using CloudServices.Common;
    using CloudServices.Common.ResponseTemplates;
    using IronMQ;
    using IronMQ.Data;
    using System.Text;
    using System.IO;
    using System;
    using System.Reflection;
    using System.Web.Http;
    using System.Runtime.Serialization;

    public class AppHarborService : IAppHarborService
    {
        //// Todo: Try just return some string. 
        /* Public AppHarborService(){
            this.data = new UnitOfWork();
            }

            FromQuestion.Compile().Invoke(question);
            (MyEnum)Enum.Parse(typeOf(MyEnum) , value)
        */
        public Stream GetMessage()
        {
            WebOperationContext context = WebOperationContext.Current;
            /* if (context.IncomingRequest.ContentType.Contains("/xml"))// /josn
             {
             //WebOperationContext.Current.OutgoingResponse.Format = WebMessageFormat.Json;

             }
             */

            var client = new Client(GlobalConstatns.ProjectID, GlobalConstatns.Token);
            Queue queue = client.Queue("TelerikDemo");
            IList<Message> messages = queue.Get(20);

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
                int index = count - 1;
                while (count > 10)
                {
                    queue.DeleteMessage(messages[index]);
                    index--;
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
