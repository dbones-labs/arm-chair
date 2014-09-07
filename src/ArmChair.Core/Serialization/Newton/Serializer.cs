// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using EntityManagement;
    using IdManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Utils;

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

        public object Deserialize(string json, Type type)
        {
            IHandler handler;
            if (_serializationHandlers.TryGetValue(type, out handler))
            {
                var ctx = new SerializerContext() { Json = json };
                handler.Handle(ctx, this);
                return ctx.Entity;
            }

            
            var doc = JObject.Parse(json);
            return DeserializeFromJson(doc);
        }

        public T Deserialize<T>(string json)
        {
            return (T)Deserialize(json, typeof (T));
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

        public string Serialize<T>(T instance)
        {
            return Serialize(instance, typeof (T));
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
            jObject.RenameProperty(idField.FriendlyName.ToCamelCase(), "_id");
            jObject.RenameProperty(revField.FriendlyName.ToCamelCase(), "_rev");

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
}