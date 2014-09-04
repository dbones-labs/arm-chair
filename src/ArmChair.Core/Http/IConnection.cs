namespace ArmChair.Http
{
    using System;
    using System.Net;

    public interface IConnection
    {
        /// <summary>
        /// set the authentication to use for all request on this connection
        /// </summary>
        IAuthentication Authentication { get; set; }

        /// <summary>
        /// set the proxy to use for this connection
        /// </summary>
        /// <remarks>
        /// setting it to null will make the connection faster
        /// </remarks>
        IWebProxy Proxy { get; set; }

        /// <summary>
        /// Execute a command against this connection/endpoint
        /// </summary>
        /// <param name="request">request to execute</param>
        /// <param name="responseHandler"></param>
        void Execute(IRequest request, Action<IResponse> responseHandler);
    }
}