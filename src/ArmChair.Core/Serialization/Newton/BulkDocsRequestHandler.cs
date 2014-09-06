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
                var jsonDoc = new JObject();
                jsonDoc.Add(jsonDoc);
                jsonDoc.Add("_id", doc.Id);
                if (string.IsNullOrWhiteSpace(doc.Rev))
                {
                    continue;
                }
                jsonDoc.Add("_rev", doc.Rev);
                if (doc.Delete)
                {
                    jsonDoc.Add("_deleted", true);
                }
            }

            var jObject = new JObject {{"docs", new JArray(jsonDocs)}};
            context.Json = jObject.ToString(Formatting.None);
        }
    }
}