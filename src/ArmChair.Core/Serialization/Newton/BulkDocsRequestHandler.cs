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