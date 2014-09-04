using System;
using Castle.DynamicProxy;

namespace ArmChair.Castle
{
    public class DocumentInterceptor : IInterceptor, IDocument
    {
        private readonly Type _documentType = typeof (IDocument);

        public void Intercept(IInvocation invocation)
        {
            Type declaringType = invocation.Method.DeclaringType;
            if (declaringType != _documentType)
            {
                invocation.Proceed();
                return;
            }

            if (!invocation.Method.IsSpecialName)
            {
                //WTF
                invocation.Proceed();
                return; 
            }
            
            var name = invocation.Method.Name;
            if (name.EndsWith("CouchDbId"))
            {
                if (name.StartsWith("set_"))
                {
                    CouchDbId = (string)invocation.Arguments[0];
                }
                else
                {
                    invocation.ReturnValue = CouchDbId;
                }
            }
            else
            {
                if (name.StartsWith("set_"))
                {
                    CouchDbVersion = (string)invocation.Arguments[0];
                }
                else
                {
                    invocation.ReturnValue = CouchDbVersion;
                }
            }

        }

        public string CouchDbId { get; set; }
        public string CouchDbVersion { get; set; }
    }
}