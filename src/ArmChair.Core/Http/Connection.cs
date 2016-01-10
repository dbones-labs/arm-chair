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