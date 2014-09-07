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
    using System.Linq;
    using Commands;
    using Newtonsoft.Json.Linq;

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
}