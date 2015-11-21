namespace ForAppHarborService
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IAppHarborService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "messages/receive")] /* ResponseFormat = WebMessageFormat.Json*/
        Stream GetMessage();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "messages/send")]
        Stream SendMessage();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "messages/sendMessage", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json)]
        void SendRealMessageMessage(ComplexStiupidShit data); // names must match and must be string !!
    }
}
