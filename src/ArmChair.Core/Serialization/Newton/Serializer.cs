// Copyright 2013 - 2015 dbones.co.uk (David Rundle)
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
    using System.Text;
    using EntityManagement;
    using Newtonsoft.Json;


    /// <summary>
    /// default serializer.
    /// </summary>
    public class Serializer : ISerializer
    {
        private JsonSerializerSettings _settings;
        private JsonSerializer _jsonSerializer;

        public Serializer(IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            var factory = new SerializerSettingsFactory();
            factory.SetUpDocumentContractResolver(idAccessor, revisionAccessor);
            var settings = factory.Create();
            _settings = settings;
            _jsonSerializer = JsonSerializer.Create(_settings);
        }

        public Serializer(JsonSerializerSettings settings)
        {
            _settings = settings;
            _jsonSerializer = JsonSerializer.Create(_settings);
        }

        /// <summary>
        /// change the settings.
        /// </summary>
        public JsonSerializerSettings Settings
        {
            set
            {
                if (value == null)
                {
                    throw new Exception("requires a value");
                }
                _settings = value;
                _jsonSerializer = JsonSerializer.Create(_settings);
            }
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