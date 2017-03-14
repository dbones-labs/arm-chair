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
    using System.Net;
    using System.Net.Http;

    /// <summary>
    /// RFC 2109 - cookie authentication.
    /// </summary>
    public class CookieAuthentication : IAuthentication
    {
        private Cookie _authSession;


        /// <summary>
        /// create a RFC 2019 (COOKIE) Authentication, to be applied on a connection
        /// </summary>
        /// <param name="serverUrl">cookie auth endpoint</param>
        /// <param name="userName">username</param>
        /// <param name="password">passwork</param>
        public CookieAuthentication(
            string serverUrl,
            string userName,
            string password)
        {

            var connection = new Connection(serverUrl);
            var request = new FormRequest("/_session", HttpMethod.Post);
            request.AddBodyParameter("name", userName);
            request.AddBodyParameter("password", password);

            using(var response = connection.Execute(request))
            {
                if (response.Status == HttpStatusCode.OK)
                {
                    _authSession = connection.Cookies.GetCookies(new Uri(serverUrl))["AuthSession"];
                }
            }
        }

        public virtual void Apply(IConnection conn)
        {
            conn.SetupConfig(config => config.CookieContainer.Add(new Uri(_authSession.Path), _authSession));
        }
    }
}