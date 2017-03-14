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
    using System.Text;
    using System.Net.Http.Headers;

    /// <summary>
    /// Http Request
    /// </summary>
    public class Request : IRequest
    {
        protected string _url;
        private readonly HttpMethod _verbType;
        protected readonly List<Parameter> _params = new List<Parameter>();
        protected readonly List<Action<HttpRequestMessage>> _configs = new List<Action<HttpRequestMessage>>();
        protected Func<HttpContent> _writeContent;

        /// <summary>
        /// create new HTTP request, you need to execute this against the connection
        /// </summary>
        /// <param name="url">addtional URL, which is applied off the URL Base on the connection</param>
        /// <param name="verbType">HTTP verb</param>
        public Request(string url, HttpMethod verbType = null)
        {
            _url = url;
            _verbType = verbType ?? HttpMethod.Get;
            CreateResponse = httpResponse => new Response(httpResponse);
        }

        public virtual Func<HttpResponseMessage, IResponse> CreateResponse { get; set; }

        public virtual string Url => _url;
        public virtual IEnumerable<Parameter> Parameters => _params;


        public virtual IResponse Execute(HttpClient connection)
        {
            var message = new HttpRequestMessage(_verbType, CreateUrl());

            if (!(_verbType == HttpMethod.Get || _writeContent == null))
            {
                message.Content = _writeContent();
            }

            Setup(message);
            return CreateResponse(connection.SendAsync(message).Result);
        }

        public virtual void AddCookie(Cookie cookie)
        {
            //TODO
            //http://stackoverflow.com/questions/28946044/httprequestmessage-changes-cookie-value
            throw new NotImplementedException();
        }

        public virtual void Configure(Action<HttpRequestMessage> config)
        {
            _configs.Add(config);
        }


        public virtual void AddParameter(Parameter param)
        {
            _params.Add(param);
        }

        public virtual void AddUrlSegment(Parameter param)
        {
            _url = _url.Replace($":{param.Name}", param.EncodedValue);
        }

        public virtual void AddContent(Func<HttpContent> writeConent, MediaTypeHeaderValue contentType = null)
        {
            if (contentType != null)
            {
                SetContentType(contentType);
            }
            _writeContent = writeConent;
        }

        protected virtual string CreateUrl()
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_url))
            {
                //try and ensure there is a /
                if (!_url.StartsWith("/"))
                {
                    builder.Append("/");
                }

                builder.Append(_url);
            }


            if (_params.Any())
            {
                var p1 = _params.Select(param => string.Format("{0}={1}", param.Name, param.EncodedValue));
                var @params = string.Join("&", p1);
                builder.Append("?");
                builder.Append(@params);
            }

            return builder.ToString();
        }

        protected virtual void Setup(HttpRequestMessage request)
        {
            foreach (var config in _configs)
            {
                config(request);
            }
        }

        public virtual void SetContentType(MediaTypeHeaderValue contentType)
        {
            _configs.Add(request =>
            {
                if (request.Content != null)
                    request.Content.Headers.ContentType = contentType;
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType.MediaType));
            });
        }

        public override string ToString()
        {
            return $"Verb: {_verbType} - {_url}.";
        }
    }
}