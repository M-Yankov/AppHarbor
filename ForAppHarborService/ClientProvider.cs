namespace ForAppHarborService
{
    using CloudServices.Common;
    using IronMQ;

    public static class ClientProvider
    {
        private static Client client;

        public static Client GetClient()
        {
            if (client == null)
            {
                client = new Client(GlobalConstatns.ProjectID, GlobalConstatns.Token);
            }

            return client;
        }
    }
}