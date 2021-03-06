﻿// Copyright 2014 - dbones.co.uk (David Rundle)
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
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    public class Response : IResponse
    {
        protected readonly HttpResponseMessage _httpResponse;
        protected string _body;

        /// <summary>
        /// Create a new request (you should not need to call this directly)
        /// </summary>
        /// <param name="response">the http responce, whcih will be wrappered in here</param>
        /// <param name="contentReader">the content reader to read the http body</param>
        public Response(HttpResponseMessage httpResponse)
        {
            if (httpResponse == null) throw new ArgumentNullException(nameof(httpResponse));

            _httpResponse = httpResponse;
            BaseContent = httpResponse.Content.ReadAsStreamAsync().Result;

            if (!string.IsNullOrWhiteSpace(httpResponse.Content.Headers.ContentType.CharSet))
            {
                var encoding = Encoding.GetEncoding(httpResponse.Content.Headers.ContentType.CharSet);
                Content = new StreamReader(BaseContent, encoding);
            }
            else
            {
                Content = new StreamReader(BaseContent);
            }

            Status = httpResponse.StatusCode;
            NumberOfBytes = httpResponse.Content.Headers.ContentLength;
            Headers = httpResponse.Content.Headers;
        }

        public virtual HttpResponseMessage InternalResponse => _httpResponse;
        public virtual HttpContentHeaders Headers { get; protected set; }
        public virtual HttpStatusCode Status { get; protected set; }
        public virtual StreamReader Content { get; protected set; }
        public virtual Stream BaseContent { get; protected set; }

        public virtual long? NumberOfBytes { get; protected set; }

        public virtual string GetBody()
        {
            return _body ?? (Content == null ? (_body = null) : (_body = Content.ReadToEnd()));
        }

        public virtual void Dispose()
        {
            Content?.Dispose();
            //TODO: will look into this.
            (_httpResponse as IDisposable)?.Dispose();
        }
    }
}