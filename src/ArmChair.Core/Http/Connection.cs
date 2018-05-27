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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class Connection : IConnection
    {
        private readonly List<Action<HttpClientHandler>> _configHandlers = new List<Action<HttpClientHandler>>();
        private readonly List<Action<HttpRequestHeaders>> _headerHandlers = new List<Action<HttpRequestHeaders>>();
        readonly HttpClientHandler _config;
        private IAuthentication _authentication;

        public string BaseUrl { get; protected set; }

        /// <summary>
        /// create a new connection instance, note that this does not keep open an active connection
        /// </summary>
        /// <param name="baseUrl">the base url which all requests will be based from</param>
        public Connection(string baseUrl)
        {
            BaseUrl = baseUrl;

            _config = new HttpClientHandler()
            {
                AutomaticDecompression =
                    DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
                //MaxConnectionsPerServer = 100,
            };
        }

        public virtual IAuthentication Authentication
        {
            get => _authentication;
            set
            {
                _authentication = value;
                Authentication?.Apply(this);
            }
        }


        public virtual IResponse Execute(IRequest request)
        {
            try
            {
                //apply once
                if (_configHandlers.Any())
                {
                    foreach (var configHandler in _configHandlers)
                    {
                        configHandler(_config);
                    }
                    _configHandlers.Clear();
                }

                var conn = new HttpClient(_config);
                conn.BaseAddress = new Uri(BaseUrl);

                foreach (var headerHandler in _headerHandlers)
                {
                    headerHandler(conn.DefaultRequestHeaders);
                }
                
                return request.Execute(conn);
            }
            catch (Exception ex)
            {
                throw new RequestException(request, ex);
            }
        }

        public CookieContainer Cookies => _config.CookieContainer;

        public void SetupConfig(Action<HttpClientHandler> config)
        {
            _configHandlers.Add(config);
        }

        public void SetupHeaders(Action<HttpRequestHeaders> defaultHeaders)
        {
            _headerHandlers.Add(defaultHeaders);
        }
    }
}