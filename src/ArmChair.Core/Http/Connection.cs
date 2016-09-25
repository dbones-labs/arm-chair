// Copyright 2014 - dbones.co.uk (David Rundle)
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

    public class Connection : IConnection
    {
        public string BaseUrl { get; protected set; }

        /// <summary>
        /// create a new connection instance, note that this does not keep open an active connection
        /// </summary>
        /// <param name="baseUrl">the base url which all requests will be based from</param>
        public Connection(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public virtual IAuthentication Authentication { get; set; }

        public virtual IWebProxy Proxy { get; set; }

        public virtual void Execute(IRequest request, Action<IResponse> responseHandler)
        {
            try
            {
                Authentication?.Apply(this, request);
                using (var response = request.Execute(BaseUrl, Proxy))
                {
                    responseHandler(response);
                }
            }
            catch (Exception ex)
            {
                throw new RequestException(request, ex);
            }

        }

        public virtual IResponse Execute(IRequest request)
        {
            try
            {
                Authentication?.Apply(this, request);
                return request.Execute(BaseUrl, Proxy);
            }
            catch (Exception ex)
            {
                throw new RequestException(request, ex);
            }

        }
    }
}