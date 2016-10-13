namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Linq;
    using Commands;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// handle the mongo query response
    /// </summary>
    public class MongoQueryResponseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Func<JObject, object> getObject = jInstance =>
            {
                var type = Type.GetType((string)jInstance["$type"]);
                using (var tReader = jInstance.CreateReader())
                {
                    return serializer.Deserialize(tReader, type);
                }
            };

            var jsonContent = JToken.ReadFrom(reader);

            var docs = jsonContent["docs"]
                .Where(x => x != null)
                .Cast<JObject>()
                .ToList();

            var entities = docs.Select(getObject).ToList();
            

            return new MongoQueryResponse()
            {
                Docs = entities
            };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(MongoQueryResponse) == objectType;
        }
    }
}