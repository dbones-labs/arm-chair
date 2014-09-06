namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Newtonsoft.Json.Linq;

    public class AllDocsResponseHandler : IHandler
    {
        public Type HandlesType { get { return typeof(AllDocsResponse); } }
        public void Handle(SerializerContext context, Serializer serializer)
        {
            
            var jsonContent = JObject.Parse(context.Json);
            var docs = jsonContent["rows"].Select(row => row["doc"]).Cast<JObject>().ToList();
            var entities = docs.Select(serializer.DeserializeFromJson).ToList();
            IEnumerable<AllDocsRowResponse> rows = entities.Select(x => new AllDocsRowResponse()
            {
                Doc = x,
            });

            context.Entity = new AllDocsResponse()
            {
                Rows = rows,
                Offset = (int)jsonContent["offset"],
                TotalRows = (int)jsonContent["total_rows"]

            };
        }
    }
}