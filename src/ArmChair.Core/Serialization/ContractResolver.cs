namespace ArmChair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using EntityManagement;
    using IdManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using Utils;

    public interface ISerializer
    {
        object Deserialize(string json);
        object Deserialize(string json, Type type);
        string Serialize(object instance);
        string Serialize(object instance, Type type);
        
    }

    public class Serializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;
        private readonly JsonSerializer _jsonSerializer;
        private readonly IDictionary<Type, IHandler> _serializationHandlers = new Dictionary<Type, IHandler>(); 


        public Serializer(JsonSerializerSettings settings, IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            _settings = settings;
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
            _jsonSerializer = JsonSerializer.Create(_settings);

            AddHandler(new AllDocsRequestHandler());
            AddHandler(new AllDocsResponseHandler());
            AddHandler(new BulkDocsRequestHandler());
            AddHandler(new BulkDocResponseHandler());

        }

        public object Deserialize(string json)
        {
            using (var stringReader = new StreamReader(json))
            using (var reader = new JsonTextReader(stringReader))
            {
                return _jsonSerializer.Deserialize(reader);
            }
        }

        public string Serialize(object instance)
        {
            var result = new StringBuilder();
            using (var stringWriter = new StringWriter(result))
            using (var writer = new JsonTextWriter(stringWriter))
            {
                _jsonSerializer.Serialize(writer, instance);
                writer.Flush();
            }
            return result.ToString();
        }


        public string Serialize(object instance, Type type)
        {
            IHandler handler;
            if (_serializationHandlers.TryGetValue(type, out handler))
            {
                var ctx = new SerializerContext() {Entity = instance};
                handler.Handle(ctx, this);
                return ctx.Json;
            }

            //default handle.
            return Serialize(instance);
        }

        public object Deserialize(string json, Type type)
        {
            IHandler handler;
            if (_serializationHandlers.TryGetValue(type, out handler))
            {
                var ctx = new SerializerContext() { Json = json };
                handler.Handle(ctx, this);
                return ctx.Entity;
            }

            //default handle.
            return Deserialize(json);
        }

        public JToken SerializeAsJson(object instance)
        {

            //TODO: just remove the ID.. and handle it locally
            var type = instance.GetType();
            JObject jObject;

            using (var writer = new JTokenWriter())
            {
                _jsonSerializer.Serialize(writer, instance);
                writer.Flush();
                jObject = (JObject) writer.Token;
            }

            var idField = _idAccessor.GetIdField(type);
            var revField = _revisionAccessor.GetRevisionField(type);
            jObject.RenameProperty(idField.FriendlyName, "_id");
            jObject.RenameProperty(revField.FriendlyName, "_rev");

            return jObject;
        }

        public object DeserializeFromJson(JObject jObject)
        {
            var typeName = (string)jObject["$type"];
            var type = Type.GetType(typeName);

            var idField = _idAccessor.GetIdField(type);
            var revField = _revisionAccessor.GetRevisionField(type);
            jObject.RenameProperty("_id", idField.FriendlyName);
            jObject.RenameProperty("_rev", revField.FriendlyName);

            using (var reader = new JTokenReader(jObject))
            {
                return _jsonSerializer.Deserialize(reader);
            }
        }

        internal void AddHandler(IHandler handler)
        {
            _serializationHandlers.Add(handler.HandlesType, handler);
        }
    }


    public class BulkDocsRequest
    {
        public IEnumerable<BulkDocRequest> Docs { get; set; }
    }

    public class BulkDocRequest
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public bool Delete { get; set; }
    }

    public class BulkDocResponse
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public bool Ok { get; set; }
    }

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



    public class AllDocsRequest
    {
        public IEnumerable<string> Keys { get; set; }
    }

    public class AllDocsResponse
    {
        public int Offset { get; set; }
        public int TotalRows { get; set; }
        public IEnumerable<AllDocsRowResponse> Rows { get; set; }
    }

    public class AllDocsRowResponse
    {
        public AllDocsValueResponse Value { get; set; }
        public object Doc { get; set; }
    }

    public class AllDocsValueResponse
    {
        public string Rev { get; set; }
    }


    public class SerializerContext
    {
        public string Json { get; set; }
        public object Entity { get; set; }
    }

    public interface IHandler
    {
        Type HandlesType { get; }
        void Handle(SerializerContext context, Serializer serializer);
    }

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
    

    public static class JObjectExtensions
    {
        public static void RenameProperty(this JObject jObject, string oldName, string newName)
        {
            var property = jObject.Property(oldName);
            var value = property.Value;
            property.Remove();

            jObject.Add(newName, value);
        }
    }

    public class ContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            //ignore the current MemberSerialization
            return base.CreateProperties(type, MemberSerialization.Fields);
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return null;
            }

            if (propertyName.StartsWith("<", StringComparison.CurrentCultureIgnoreCase))
            {
                int index = propertyName.IndexOf(">", StringComparison.CurrentCultureIgnoreCase);
                propertyName = propertyName.Substring(1, index - 1);
            }

            if (propertyName.StartsWith("_", StringComparison.CurrentCultureIgnoreCase))
            {
                propertyName = propertyName.Substring(1, propertyName.Length - 1);
            }

            return base.ResolvePropertyName(propertyName);
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var temp = objectType.GetTypeMeta().Fields.Select(x => x.FieldInfo).Cast<MemberInfo>().ToList();
            return temp;
        }
    }
}