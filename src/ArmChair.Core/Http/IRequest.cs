// Copyright 2013 - 2015 dbones.co.uk (David Rundle)
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

    /// <summary>
    /// a http request
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// proxy to use with the request
        /// </summary>
        IWebProxy Proxy { get; set; }

        /// <summary>
        /// execute the request
        /// </summary>
        /// <param name="baseUrl">the base URL which the request will be against</param>
        /// <param name="handleResonse">delegate to handle the response</param>
        /// <param name="proxy">override the proxy, else use the one which is registered against the request</param>
        void Execute(string baseUrl, Action<Response> handleResonse, IWebProxy proxy = null);
        
        /// <summary>
        /// add a header
        /// </summary>
        /// <param name="key">the name</param>
        /// <param name="header">the value</param>
        void AddHeader(string key, string header);
        
        /// <summary>
        /// add a cookie 
        /// </summary>
        /// <param name="cookie"></param>
        void AddCookie(Cookie cookie);

        /// <summary>
        /// add a URL param (?name=value)
        /// </summary>
        /// <param name="key">the name</param>
        /// <param name="value">value</param>
        void AddParameter(string key, string value);

        /// <summary>
        /// add URL param hi.com/:name => hi.com/value
        /// </summary>
        /// <param name="key">param name</param>
        /// <param name="value">the param value</param>
        void AddUrlSegment(string key, string value);

        /// <summary>
        /// add the content/body to the request
        /// </summary>
        /// <param name="writeConent">write to the steam when it has been made avaliable</param>
        /// <param name="contentType">the type of the content</param>
        void AddContent(Action<StreamWriter> writeConent, HttpContentType contentType);
    }
}