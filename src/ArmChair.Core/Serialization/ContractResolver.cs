namespace ArmChair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using EntityManagement;
    using IdManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using Utils;

    public interface ISerializer
    {
        object Deserialize(string json);
        string Serialize(object instance);
        string Serialize<T>(T instance);
        T Deserialize<T>(string json);
    }

    public class Serializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;
        private JsonSerializer _jsonSerializer;

        public Serializer(JsonSerializerSettings settings, IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            _settings = settings;
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
            _jsonSerializer = JsonSerializer.Create(_settings);
        }

        public object Deserialize(string json)
        {
            var jobject = JObject.Parse(json);

            


            using (var reader = new JsonTextReader(new StreamReader(json)))
            {
            //return _jsonSerializer    
            }
            throw new NotImplementedException();
        }

        public string Serialize(object instance)
        {
            throw new NotImplementedException();

        }


        public string Serialize<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string json)
        {
            throw new NotImplementedException();
        }

        public JToken SerializeAsJson(object instance)
        {
            
        }

        public object DeserializeFromJson(JToken instance)
        {

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

    public abstract class HandlerBase : IHandler
    {
        public IIdAccessor IdAccessor { get; internal set; }
        public IRevisionAccessor RevisionAccessor { get; internal set; }

        public abstract Type HandlesType { get; }
        public abstract void Handle(SerializerContext context, Serializer serializer);

        public JToken SerializeToJson(object instance, Serializer serializer)
        {
            var type = instance.GetType();
            var jObject = (JObject)serializer.SerializeAsJson(instance);
            
            var idField = IdAccessor.GetIdField(type);
            var revField = RevisionAccessor.GetRevisionField(type);
            jObject.RenameProperty(idField.FriendlyName, "_id");
            jObject.RenameProperty(revField.FriendlyName, "_rev");

            return jObject;

        }

        public object DeserializeInstance(JObject jObject, Serializer serializer)
        {
            var typeName = (string)jObject["$type"];
            var type = Type.GetType(typeName);
            
            var idField = IdAccessor.GetIdField(type);
            var revField = RevisionAccessor.GetRevisionField(type);
            jObject.RenameProperty("_id", idField.FriendlyName);
            jObject.RenameProperty("_rev", revField.FriendlyName);

            return serializer.DeserializeFromJson(jObject);
        }
    }

    public class AllDocsRequestHandler : IHandler
    {
        public Type HandlesType { get { return typeof (AllDocsRequest); } }
        public void Handle(SerializerContext context, Serializer serializer)
        {
            var request = (AllDocsRequest) context.Entity;
            var keys = serializer.SerializeAsJson(request.Keys);

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
        public static void RenameKeyFields(this JObject jObject, IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            var typeName = (string)jObject["$type"];
            if (!string.IsNullOrWhiteSpace(typeName))
            {
                var type = Type.GetType(typeName);
                var idField = idAccessor.GetIdField(type);
                if (idField == null)
                {
                    return;
                }
                var revisionField = revisionAccessor.GetRevisionField(type);

            }
        }

        public static void RenameProperty(this JObject jObject, string oldName, string newName)
        {
            

            //var temp = jObject[oldName];
            //jObject.Remove(temp)
        }
    }

    public class DocumentJsonWriter : JsonTextWriter
    {
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        public DocumentJsonWriter(TextWriter textWriter, IIdAccessor idAccessor, IRevisionAccessor revisionAccessor) 
            : base(textWriter)
        {
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
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