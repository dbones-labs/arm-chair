namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Newtonsoft.Json.Linq;

    public class BulkDocResponseHandler : IHandler
    {
        public Type HandlesType { get { return typeof(IEnumerable<BulkDocResponse>); } }
        public void Handle(SerializerContext context, Serializer serializer)
        {
            var response = context.Json;
            var json = new JArray(response);

            context.Entity = json.Select(x => new BulkDocResponse()
            {
                Id = (string)x["id"],
                Rev = (string)x["rev"],
                Ok = (bool)x["ok"]
            }).ToList();
        }
    }
}