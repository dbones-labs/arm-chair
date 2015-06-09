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
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;


    /// <summary>
    /// handle the query response
    /// </summary>
    public class AllDocsResponseConverter : JsonConverter
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
                Offset = (int)jsonContent["offset"],
                TotalRows = (int)jsonContent["total_rows"]
            };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(AllDocsResponse) == objectType;
        }
    }
}