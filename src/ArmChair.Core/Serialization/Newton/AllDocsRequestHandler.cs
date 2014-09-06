namespace ArmChair.Serialization.Newton
{
    using System;
    using Commands;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class AllDocsRequestHandler : IHandler
    {
        public Type HandlesType { get { return typeof (AllDocsRequest); } }
        public void Handle(SerializerContext context, Serializer serializer)
        {
            var request = (AllDocsRequest) context.Entity;
            var keys = serializer.Serialize(request.Keys);

            var jObject = new JObject();
            jObject.Add("keys", keys);

            context.Json = jObject.ToString(Formatting.None);
        }
    }
}