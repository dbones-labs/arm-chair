namespace ArmChair.Http
{
    using System.IO;
    using System.Net;

    public class Response : IResponse
    {


        public Response(HttpWebResponse response, StreamReader contentReader = null)
        {
            Content = contentReader;
            Status = response.StatusCode;
            NumberOfBytes = response.ContentLength;
            Headers = response.Headers;
            Cookies = response.Cookies;
        }

        public CookieCollection Cookies { get; set; }
        public WebHeaderCollection Headers { get; private set; }
        public HttpStatusCode Status { get; private set; }
        public StreamReader Content { get; private set; }
        public long NumberOfBytes { get; private set; }




    }
}