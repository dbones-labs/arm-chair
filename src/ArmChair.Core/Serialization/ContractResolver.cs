namespace ArmChair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using EntityManagement;
    using IdManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using Utils;

    public class Serializer
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

        public static void RenameProperty<T>(this JObject jObject, string oldName, string newName)
        {
            //var temp = (T)jObject[oldName];
            //jObject.Remove(temp)
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