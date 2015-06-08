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
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Text;
    using EntityManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Serializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;
        private readonly JsonSerializer _jsonSerializer;


        public Serializer(IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
            : this(new JsonSerializerSettings
                {
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                    TypeNameHandling = TypeNameHandling.Objects,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new JsonConverter[]{
                        new IsoDateTimeConverter(),
                        new BulkDocsRequestConverter(), 
                        new BulkDocsResponseConverter(), 
                        new AllDocsRequestConverter(), 
                        new AllDocsResponseConverter() },
                    ContractResolver = new ContractResolver(idAccessor, revisionAccessor)
                })
        {
        }

        public Serializer(JsonSerializerSettings settings)
        {
            _settings = settings;
            _jsonSerializer = JsonSerializer.Create(_settings);
        }

        public object Deserialize(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var reader = new JsonTextReader(stringReader))
            {
                return _jsonSerializer.Deserialize(reader);
            }
        }

        public object Deserialize(string json, Type type)
        {
            using (var stringReader = new StringReader(json))
            using (var reader = new JsonTextReader(stringReader))
            {
                return _jsonSerializer.Deserialize(reader, type);
            }
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
            return Serialize(instance);
        }

        public string Serialize<T>(T instance)
        {
            return Serialize(instance, typeof(T));
        }
    }
}