namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// add a _types to each object which is being saved.
    /// </summary>
    public class DocConverter : JsonConverter
    {
        private HashSet<Type> _toSkip;
        public DocConverter()
        {
            _toSkip = new HashSet<Type>
            {
                typeof(MongoQueryRequest),
                typeof(AllDocsRequest),
                typeof(BulkDocRequest),
                typeof(BulkDocsRequest)
            };
        }

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

            var docs = jsonContent["rows"]
                .Select(row => row["doc"])
                .Where(x => x != null)
                .Cast<JObject>()
                .ToList();

            var entities = docs.Select(getObject).ToList();
            IEnumerable<AllDocsRowResponse> rows = entities.Select(x => new AllDocsRowResponse()
            {
                Doc = x,
            });

            return new AllDocsResponse()
            {
                Rows = rows,
                Offset = (int)(jsonContent["offset"] ?? 0),
                TotalRows = (int)jsonContent["total_rows"]
            };
        }

        public override bool CanWrite => true;

        public override bool CanRead { get; } = false;

        public override bool CanConvert(Type objectType)
        {
            return !_toSkip.Contains(objectType);
        }
    }
}