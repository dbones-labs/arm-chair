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
namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class BulkDocsRequestHandler : IHandler
    {
        public Type HandlesType { get { return typeof(BulkDocsRequest); } }
        public void Handle(SerializerContext context, Serializer serializer)
        {
            var request = (BulkDocsRequest)context.Entity;

            var jsonDocs = new List<JObject>();
            foreach (var doc in request.Docs)
            {
                //delete
                if (doc.Delete)
                {
                    var toRemove = new JObject();
                    toRemove.Add("_id", new JValue(doc.Id));
                    toRemove.Add("_rev", new JValue(doc.Rev));
                    toRemove.Add("_deleted", true);
                    jsonDocs.Add(toRemove);
                    continue;
                }

                var jsonDoc = (JObject)serializer.SerializeAsJson(doc.Content);
                jsonDocs.Add(jsonDoc);
             
                //add
                if (string.IsNullOrWhiteSpace((string)jsonDoc["_rev"]))
                {
                    //new doc
                    jsonDoc.Remove("_rev");
                }
            }

            var jObject = new JObject {{"docs", new JArray(jsonDocs)}};
            context.Json = jObject.ToString(Formatting.None);
        }
    }
}