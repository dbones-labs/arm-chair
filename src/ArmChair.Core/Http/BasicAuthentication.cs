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
    using System.Net.Http.Headers;
    using System.Text;


    /// <summary>
    /// RFC 2617 - basic authentication.
    /// </summary>
    public class BasicAuthentication : IAuthentication
    {
        private readonly string _authHeader;
        /// <summary>
        /// Create an instance of a RFC 2617 (aka BASIC) Authentication, to be applied on a connection
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        public BasicAuthentication(string userName, string password)
        {
            _authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));
        }

        /// <summary>
        /// applies security to the request.
        /// </summary>
        public virtual void Apply(IConnection connection)
        {
            connection.SetupHeaders(headers => headers.Authorization = new AuthenticationHeaderValue("Basic", _authHeader));
        }
    }
}
