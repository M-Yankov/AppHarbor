namespace ForAppHarborService
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IAppHarborService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/receive")]
        string GetMessage();

        [OperationContract]
        [WebInvoke(Method = "Post", UriTemplate = "/send")]
        void SendMessage(string text);
    }
}
