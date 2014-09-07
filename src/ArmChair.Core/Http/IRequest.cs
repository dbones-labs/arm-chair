﻿// // Copyright 2013 - 2014 dbones.co.uk (David Rundle)
// // 
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// // 
// //     http://www.apache.org/licenses/LICENSE-2.0
// // 
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
namespace ArmChair.Http
{
    using System;
    using System.IO;
    using System.Net;

    public interface IRequest
    {
        IWebProxy Proxy { get; set; }
        void Execute(string baseUrl, Action<Response> handleRequest, IWebProxy proxy = null);
        void AddHeader(string key, string header);
        void AddCookie(Cookie cookie);
        void AddParameter(string key, string value);
        void AddUrlSegment(string key, string value);
        void AddContent(Action<StreamWriter> writeConent, HttpContentType contentType);
    }
}