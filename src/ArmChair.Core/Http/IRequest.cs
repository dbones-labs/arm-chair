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
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    /// <summary>
    /// a http request
    /// </summary>
    public interface IRequest
    {

        /// <summary>
        /// URL of the request, not including the base
        /// </summary>
        string Url { get; }

        /// <summary>
        /// HTTP Parameters being used in the required (on the queryString)
        /// </summary>
        IEnumerable<Parameter> Parameters { get; }

//        /// <summary>
//        /// HTTP Verb being used in the request
//        /// </summary>
//        HttpVerbType HttpVerb { get; }

        /// <summary>
        /// execute the request
        /// </summary>
        IResponse Execute(HttpClient connection);

        /// <summary>
        /// access the request object directly
        /// </summary>
        /// <param name="config">setup the request object</param>
        void Configure(Action<HttpRequestMessage> config);

//        /// <summary>
//        /// add a header
//        /// </summary>
//        /// <param name="key">the name</param>
//        /// <param name="header">the value</param>
//        void AddHeader(string key, string header);

        /// <summary>
        /// add a cookie 
        /// </summary>
        /// <param name="cookie"></param>
        void AddCookie(Cookie cookie);

        /// <summary>
        /// add a URL param (?name=value)
        /// </summary>
        /// <param name="param">full spec the param</param>
        void AddParameter(Parameter param);

        /// <summary>
        /// add URL param hi.com/:name => hi.com/value
        /// </summary>
        /// <param name="param">full spec the param</param>
        void AddUrlSegment(Parameter param);

        /// <summary>
        /// add the content/body to the request
        /// </summary>
        /// <param name="writeConent">write to the steam when it has been made avaliable</param>
        /// <param name="contentType">the type of the content</param>
        void AddContent(Func<HttpContent> writeConent, MediaTypeHeaderValue contentType = null);

//        /// <summary>
//        /// over ride any value on the underlying httpRequest.
//        /// </summary>
//        /// <param name="apply"></param>
//        void SetupRequest(Action<HttpClient> apply);
    }


    public static class RequestExtensions
    {
        /// <summary>
        /// add a URL param (?name=value), uses a default encoder
        /// </summary>
        /// <param name="name">the name</param>
        /// <param name="value">value</param>
        public static void AddParameter(this IRequest request, string name, string value)
        {
            request.AddParameter(new Parameter { Name = name, Value = value} );
        }

        /// <summary>
        /// add URL param hi.com/:name => hi.com/value (uses default encoder)
        /// </summary>
        /// <param name="name">param name</param>
        /// <param name="value">the param value</param>
        public static void AddUrlSegment(this IRequest request, string name, string value)
        {
            request.AddUrlSegment(new Parameter { Name = name, Value = value} );
        }
    }
}