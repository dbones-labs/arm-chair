// // Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
    using System.Net;

    /// <summary>
    /// RFC 2109
    /// </summary>
    public class CookieAuthentication : IAuthentication
    {
        private Cookie _authSession;


        public CookieAuthentication(
            string serverUrl,
            string userName,
            string password)
        {

            var connection = new Connection(serverUrl);
            var request = new FormRequest("/_session", HttpVerbType.Post);
            request.AddFormParameter("name", userName);
            request.AddFormParameter("password", password);

            connection.Execute(request, response =>
            {
                if (response.Status == HttpStatusCode.OK)
                {
                    _authSession = response.Cookies["AuthSession"];
                }

            });
        }

        public void Apply(IRequest request)
        {
            request.AddCookie(_authSession);
        }
    }
}