namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
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
                var jsonDoc = (JObject)serializer.SerializeAsJson(doc.Content);
                jsonDocs.Add(jsonDoc);
             
                if (string.IsNullOrWhiteSpace((string)jsonDoc["_rev"]))
                {
                    //new doc
                    jsonDoc.Remove("_rev");
                    continue;
                }
                

                if (doc.Delete)
                {
                    var toRemove = jsonDoc.Properties().Where(x => x.Name != "_id" || x.Name != "_rev").ToList();
                    foreach (var prop in toRemove)
                    {
                        prop.Remove();
                    }

                    jsonDoc.Add("_deleted", true);
                }
            }

            var jObject = new JObject {{"docs", new JArray(jsonDocs)}};
            context.Json = jObject.ToString(Formatting.None);
        }
    }
}