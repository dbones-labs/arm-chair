// Copyright 2014 - dbones.co.uk (David Rundle)
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
    using System.Reflection;
    using Commands;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// handle the Bulk add, insert and delete request
    /// </summary>
    public class BulkDocsResponseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var json = JToken.ReadFrom(reader);

            return json.Children().Select(x => new BulkDocResponse()
            {
                Id = (string)x["id"],
                Rev = (string)x["rev"],
                Ok = (bool)x["ok"]
            }).ToList();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<BulkDocResponse>).GetTypeInfo().IsAssignableFrom(objectType);
        }
    }
}