// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace ArmChair.Http
{
    using System;
    using System.Net;

    /// <summary>
    /// HTTP connection, which requests are excuted against.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// set the authentication to use for all request on this connection.
        /// </summary>
        IAuthentication Authentication { get; set; }

        /// <summary>
        /// set the proxy to use for this connection.
        /// </summary>
        /// <remarks>
        /// setting it to null will make the connection faster.
        /// </remarks>
        IWebProxy Proxy { get; set; }

        /// <summary>
        /// Execute a command against this connection/endpoint.
        /// </summary>
        /// <param name="request">request to execute.</param>
        /// <param name="responseHandler">handler of the response</param>
        void Execute(IRequest request, Action<IResponse> responseHandler);
    }
}