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

    /// <summary>
    /// Http response
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// HTTP status, ie 200
        /// </summary>
        HttpStatusCode Status { get; }

        /// <summary>
        /// the content stream
        /// </summary>
        StreamReader Content { get; }

        /// <summary>
        /// the size of the response
        /// </summary>
        long NumberOfBytes { get; }

        /// <summary>
        /// cookies
        /// </summary>
        CookieCollection Cookies { get; set; }

        /// <summary>
        /// headers
        /// </summary>
        WebHeaderCollection Headers { get; }
    }
}