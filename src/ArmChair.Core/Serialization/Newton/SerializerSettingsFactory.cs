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
    using System.Collections.Generic;
    using System.Runtime.Serialization.Formatters;
    using EntityManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// tries to help setup the Json Serializer Settings
    /// </summary>
    public class SerializerSettingsFactory
    {
        public List<JsonConverter> Converters { get; private set; }

        public IContractResolver ContractResolver { get; set; }

        /// <summary>
        /// remember to call the setup and create functions, and add any converters you need
        /// </summary>
        public SerializerSettingsFactory()
        {
            Converters = new List<JsonConverter>(5);
            Converters.AddRange(new JsonConverter[]{
                new IsoDateTimeConverter(),
                new BulkDocsRequestConverter(), 
                new BulkDocsResponseConverter(), 
                new AllDocsRequestConverter(), 
                new AllDocsResponseConverter() });
                        
        }

        public void SetUpDocumentContractResolver(IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            ContractResolver = new DocumentContractResolver(idAccessor, revisionAccessor);
        }

        public JsonSerializerSettings Create()
        {
            return new JsonSerializerSettings()
            {
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = Converters,
                ContractResolver = ContractResolver  
            };
        }
    }
}