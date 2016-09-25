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
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Http Request
    /// </summary>
    public class Request : IRequest
    {
        protected string _url;
        protected readonly List<Parameter> _params = new List<Parameter>();
        protected readonly List<Action<HttpWebRequest>> _configs = new List<Action<HttpWebRequest>>();
        protected Action<StreamWriter> _writeContent;
        protected HttpVerbType _httpVerb = HttpVerbType.Get;
        protected List<Action<HttpWebRequest>> _requestSetups = new List<Action<HttpWebRequest>>();

        /// <summary>
        /// create new HTTP request, you need to execute this against the connection
        /// </summary>
        /// <param name="url">addtional URL, which is applied off the URL Base on the connection</param>
        /// <param name="verbType">HTTP verb</param>
        public Request(string url, HttpVerbType verbType = HttpVerbType.Get)
        {
            _url = url;
            SetVerb(verbType);
            CreateResponse = httpResponse => new Response(httpResponse);
        }

        public virtual IWebProxy Proxy { get; set; }
        public virtual string Url => _url;
        public virtual HttpVerbType HttpVerb => _httpVerb;
        public virtual Func<HttpWebResponse, IResponse> CreateResponse { get; set; }
        public virtual IEnumerable<Parameter> Parameters => _params;
        public virtual string UserAgent { get; set; }
        public string Accept { get; set; }


        public virtual IResponse Execute(string baseUrl, IWebProxy proxy = null)
        {
            string url = CreateUrl(baseUrl);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = proxy ?? Proxy;
            //request.KeepAlive = false;
            if (UserAgent != null) request.UserAgent = UserAgent;
            if (Accept != null) request.Accept = Accept;

            Setup(request);

            request.AutomaticDecompression =
                DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;

            if (!(_httpVerb == HttpVerbType.Get || _writeContent == null))
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    _writeContent(streamWriter);
                    streamWriter.Flush();
                }
            }

            try
            {
                return CreateResponse((HttpWebResponse)request.GetResponse());
            }
            catch (WebException we)
            {
                if (we.Response == null)
                {
                    throw;
                }

                var httpResponse = we.Response as HttpWebResponse;
                var response = CreateResponse(httpResponse);
                return response;
            }
        }
        
        public virtual void AddHeader(string key, string header)
        {
            switch (key)
            {
                case "Accept":
                    Accept = header;
                    break;
                case "User-Agent":
                    UserAgent = header;
                    break;
                default:
                    _configs.Add(request => request.Headers.Add(key, header));
                    break;
            }
        }

        public virtual void AddCookie(Cookie cookie)
        {
            _configs.Add(request => request.CookieContainer.Add(cookie));
        }

        public virtual void Configure(Action<HttpWebRequest> config)
        {
            _configs.Add(config);
        }

        public virtual void AddParameter(string key, string value)
        {
            AddParameter(new Parameter(){Name = key, Value = value});
        }

        public virtual void AddParameter(Parameter param)
        {
            _params.Add(param);
        }

        public virtual void AddUrlSegment(string key, string value)
        {
            AddUrlSegment(new Parameter(){Name = key, Value = value});
        }

        public virtual void AddUrlSegment(Parameter param)
        {
            _url = _url.Replace(string.Format(":{0}", param.Name), param.EncodedValue);
        }

        public virtual void AddContent(Action<StreamWriter> writeConent, HttpContentType contentType)
        {
            SetContentType(contentType);
            _writeContent = writeConent;
        }

        public virtual void SetupRequest(Action<HttpWebRequest> apply)
        {
            _requestSetups.Add(apply);
        }

        protected virtual string CreateUrl(string baseUrl)
        {
            var builder = new StringBuilder();
            builder.Append(baseUrl);

            if (!string.IsNullOrWhiteSpace(_url))
            {
                //try and ensure there is a /
                if (!baseUrl.EndsWith("/") && !_url.StartsWith("/"))
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

        protected virtual void Setup(HttpWebRequest request)
        {
            foreach (var config in _configs)
            {
                config(request);
            }
        }

        public virtual void SetContentType(HttpContentType contentType)
        {
            string value;
            switch (contentType)
            {
                case HttpContentType.Json:
                    value = "application/json";
                    break;
                case HttpContentType.Form:
                    value = "application/x-www-form-urlencoded";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType));
            }
            _configs.Add(request => request.ContentType = value);
        }

        public override string ToString()
        {
            return $"Verb: {_httpVerb} - {_url}.";
        }

        protected virtual void SetVerb(HttpVerbType verbType)
        {
            _httpVerb = verbType;
            string value;
            switch (verbType)
            {
                case HttpVerbType.Delete:
                    value = "DELETE";
                    break;
                case HttpVerbType.Post:
                    value = "POST";
                    break;
                case HttpVerbType.Put:
                    value = "PUT";
                    break;
                case HttpVerbType.Get:
                    value = "GET";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(verbType));
            }

            _configs.Add(request => request.Method = value);
        }
    }
}