namespace ArmChair.Http
{
    using System;
    using System.Net;

    public class Connection : IConnection
    {
        private readonly string _baseUrl;

        public Connection(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public IAuthentication Authentication { get; set; }

        public IWebProxy Proxy { get; set; }

        public void Execute(IRequest request, Action<IResponse> responseHandler)
        {
            try
            {
                if (Authentication != null)
                {
                    Authentication.Apply(request);
                }
                request.Execute(_baseUrl, responseHandler, Proxy);
            }
            catch (Exception ex)
            {
                throw new RequestException(request, ex);
            }

        }
    }
}