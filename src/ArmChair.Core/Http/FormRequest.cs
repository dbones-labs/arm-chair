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
    using System.Linq;
    using System.Net;

    /// <summary>
    /// A HTTP form request
    /// </summary>
    public class FormRequest : Request
    {
        protected List<Parameter> _formParams = new List<Parameter>();

        public FormRequest(string appendUrlToBase, HttpVerbType verbType = HttpVerbType.Post)
            : base(appendUrlToBase, verbType)
        {
            SetContentType(HttpContentType.Form);
            if (verbType == HttpVerbType.Get)
            {
                throw new NotSupportedException("cannot use Get verb");
            }
        }

        /// <summary>
        /// body paramerters, to be sent which are to be sent to the server
        /// </summary>
        public virtual IEnumerable<Parameter> BodyParameters => _formParams;

        public override IResponse Execute(string baseUrl, IWebProxy proxy = null)
        {
            if (_formParams.Any())
            {
                var values = _formParams.Select(x => $"{x.Name}={x.EncodedValue}");
                _writeContent = writer => writer.Write(string.Join("&", values));
            }

            return base.Execute(baseUrl, proxy);
        }

        public override void AddContent(Action<StreamWriter> writeConent, HttpContentType contentType)
        {
            throw new NotImplementedException("use AddBodyParameter");
        }

        /// <summary>
        /// Add a body parameter (uses no encoder, string will be passed through)
        /// </summary>
        /// <param name="key">name of the param</param>
        /// <param name="value">value of the param</param>
        public virtual void AddBodyParameter(string key, string value)
        {
            _formParams.Add(new Parameter { Name = key, Value = value, EncodingFunc = v => v });
        }

        /// <summary>
        /// Add a body parameter
        /// </summary>
        /// <param name="param">the parameter containing the name and value</param>
        public virtual void AddBodyParameter(Parameter param)
        {
            _formParams.Add(param);
        }

    }
}