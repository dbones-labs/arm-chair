namespace ArmChair.Http
{
    using System;
    using System.IO;
    using System.Net;

    public interface IRequest
    {
        IWebProxy Proxy { get; set; }
        void Execute(string baseUrl, Action<Response> handleRequest, IWebProxy proxy = null);
        void AddHeader(string key, string header);
        void AddCookie(Cookie cookie);
        void AddParameter(string key, string value);
        void AddUrlSegment(string key, string value);
        void AddContent(Action<StreamWriter> writeConent, HttpContentType contentType);
    }
}