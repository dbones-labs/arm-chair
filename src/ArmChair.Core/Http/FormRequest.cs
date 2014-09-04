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

        public override void Execute(string baseUrl, Action<Response> handleRequest, IWebProxy proxy = null)
        {
            if (_formParams.Any())
            {
                _writeContent = writer => writer.Write(string.Join("&", _formParams));
            }

            base.Execute(baseUrl, handleRequest, proxy);
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