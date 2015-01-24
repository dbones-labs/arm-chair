// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
    using System.Web;

    public class FormRequest : Request
    {
        protected List<string> _formParams = new List<string>();

        public FormRequest(string url, HttpVerbType verbType)
            : base(url, verbType)
        {
            SetContentType(HttpContentType.Form);
            if (verbType == HttpVerbType.Get)
            {
                throw new NotSupportedException("cannot use Get verb");
            }
        }

        public override void Execute(string baseUrl, Action<Response> handleResonse, IWebProxy proxy = null)
        {
            if (_formParams.Any())
            {
                _writeContent = writer => writer.Write(string.Join("&", _formParams));
            }

            base.Execute(baseUrl, handleResonse, proxy);
        }

        public override void AddContent(Action<StreamWriter> writeConent, HttpContentType contentType)
        {
            throw new NotImplementedException("use AddFormParameter");
        }

        public virtual void AddFormParameter(string key, string value)
        {
            var encoded = HttpUtility.UrlEncode(value);
            _formParams.Add(string.Format("{0}={1}", key, encoded));
        }

    }
}