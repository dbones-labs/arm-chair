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
    using System.Web;

    public class Request : IRequest
    {
        protected string _url;
        protected readonly List<string> _params = new List<string>();
        protected readonly List<Action<HttpWebRequest>> _configs = new List<Action<HttpWebRequest>>();
        protected Action<StreamWriter> _writeContent;
        protected HttpVerbType _httpVerb = HttpVerbType.Get;

        public Request(string url, HttpVerbType verbType)
        {
            _url = url;
            SetVerb(verbType);
        }

        public virtual IWebProxy Proxy { get; set; }

        public virtual void Execute(string baseUrl, Action<Response> handleResonse, IWebProxy proxy = null)
        {
            string url = CreateUrl(baseUrl);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = proxy ?? Proxy;

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
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    Stream stream = httpResponse.GetResponseStream();
                    if (stream == null)
                    {
                        var response = new Response(httpResponse);
                        handleResonse(response);
                    }
                    else
                    {
                        using (var contentReader = new StreamReader(stream))
                        {
                            var response = new Response(httpResponse, contentReader);
                            handleResonse(response);
                        }
                    }
                }
            }
            catch (WebException we)
            {
                var httpResponse = we.Response as HttpWebResponse;
                var response = new Response(httpResponse);
                handleResonse(response);
            }
        }

        public virtual void AddHeader(string key, string header)
        {
            _configs.Add(request => request.Headers.Add(key, header));
        }

        public void AddCookie(Cookie cookie)
        {
            _configs.Add(request => request.CookieContainer.Add(cookie));
        }

        public virtual void AddParameter(string key, string value)
        {
            var encoded = HttpUtility.UrlEncode(value);
            _params.Add(string.Format("{0}={1}", key, encoded));
        }

        public virtual void AddUrlSegment(string key, string value)
        {
            var encoded = HttpUtility.UrlEncode(value);
            _url = _url.Replace(string.Format(":{0}", key), encoded);
        }

        public virtual void AddContent(Action<StreamWriter> writeConent, HttpContentType contentType)
        {
            SetContentType(contentType);
            _writeContent = writeConent;
        }

        private string CreateUrl(string baseUrl)
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
                var @params = string.Join("&", _params);
                builder.Append("?");
                builder.Append(@params);
            }

            return builder.ToString();
        }

        private void Setup(HttpWebRequest request)
        {
            foreach (var config in _configs)
            {
                config(request);
            }
        }

        public void SetContentType(HttpContentType contentType)
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
                    throw new ArgumentOutOfRangeException("contentType");
            }
            _configs.Add(request => request.ContentType = value);
        }

        private void SetVerb(HttpVerbType verbType)
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
                    throw new ArgumentOutOfRangeException("verbType");
            }

            _configs.Add(request => request.Method = value);
        }
    }
}