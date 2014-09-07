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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Utils;

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

        protected override IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
        {
            return base.CreateConstructorParameters(constructor, memberProperties);
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (!objectType.IsClass || typeof(IEnumerable).IsAssignableFrom(objectType))
            {
                return base.CreateContract(objectType);
            }

            var contract = base.CreateContract(objectType);
            contract.DefaultCreator = objectType.GetTypeMeta().Ctor;
            contract.DefaultCreatorNonPublic = true;
            return contract;
        }
    }
}