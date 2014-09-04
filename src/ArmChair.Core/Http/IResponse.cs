namespace ArmChair.Http
{
    using System.IO;
    using System.Net;

    public interface IResponse
    {
        HttpStatusCode Status { get; }
        StreamReader Content { get; }
        long NumberOfBytes { get; }
        CookieCollection Cookies { get; set; }
        WebHeaderCollection Headers { get; }
    }
}