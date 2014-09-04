namespace ArmChair.Http
{
    using System;

    public class RequestException : Exception
    {
        public RequestException(IRequest request)
        {
            Request = request;
        }

        public RequestException(IRequest request, Exception innerException)
            : base("", innerException)
        {
            Request = request;
        }

        public IRequest Request { get; set; }
    }
}