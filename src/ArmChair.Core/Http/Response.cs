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
    using System.IO;
    using System.Net;

    public class Response : IResponse
    {


        public Response(HttpWebResponse response, StreamReader contentReader = null)
        {
            Content = contentReader;
            Status = response.StatusCode;
            NumberOfBytes = response.ContentLength;
            Headers = response.Headers;
            Cookies = response.Cookies;
        }

        public CookieCollection Cookies { get; set; }
        public WebHeaderCollection Headers { get; private set; }
        public HttpStatusCode Status { get; private set; }
        public StreamReader Content { get; private set; }
        public long NumberOfBytes { get; private set; }




    }
}