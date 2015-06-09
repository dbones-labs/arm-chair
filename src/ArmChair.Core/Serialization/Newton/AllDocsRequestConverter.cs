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
    using Commands;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// handle the query requests
    /// </summary>
    public class AllDocsRequestConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Func<object, JArray> getJObjects = instance =>
            {
                using (var tWriter = new JTokenWriter())
                {
                    serializer.Serialize(tWriter, instance);
                    tWriter.Flush();
                    return (JArray)tWriter.Token;
                }
            };

            var request = (AllDocsRequest)value;
            var keys = getJObjects(request.Keys);

            var jObject = new JObject();
            jObject.Add("keys", keys);

            jObject.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(AllDocsRequest) == objectType;
        }
    }
}