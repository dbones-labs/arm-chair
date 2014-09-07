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