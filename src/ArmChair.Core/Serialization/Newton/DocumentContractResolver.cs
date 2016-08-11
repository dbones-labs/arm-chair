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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using EntityManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Utils;

    /// <summary>
    /// Armchair overrides the default contract resolver to add some support for its naming conventions
    /// </summary>
    public class DocumentContractResolver : DefaultContractResolver // CamelCasePropertyNamesContractResolver
    {
        private IDictionary<Type, LinkedList<PropertiesOverride>> _propertiesOverride = new Dictionary<Type, LinkedList<PropertiesOverride>>();
        protected PropertiesOverride _baseOverride;

        /// <summary>
        /// create an instance of the contract resolver to handle Counch DB documents 
        /// </summary>
        public DocumentContractResolver(IIdAccessor idAccessor, IRevisionAccessor revisionAccessor) : base(true)
        {
            _baseOverride = new BaseOverride(idAccessor, revisionAccessor);

            var naming = new LocalCamelCaseNamingStrategy
            {
                OverrideSpecifiedNames = true,
                ProcessDictionaryKeys = true
            };

            NamingStrategy = naming;
        }

        
        /// <summary>
        /// add a <see cref="PropertiesOverride"/> to the reslover, consider this as an alternative 
        /// to Converters
        /// </summary>
        /// <param name="override">the override to add</param>
        public virtual void AddPropertyOverride(PropertiesOverride @override)
        {
            LinkedList<PropertiesOverride> overrides = null;
            if (!_propertiesOverride.TryGetValue(@override.Type, out overrides))
            {
                overrides = new LinkedList<PropertiesOverride>();
                _propertiesOverride.Add(@override.Type, overrides);
            }
            overrides.AddLast(@override);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            //ignore the current MemberSerialization
            var properties =  base.CreateProperties(type, MemberSerialization.Fields).ToDictionary(x => x.PropertyName);
            _baseOverride.Set(type, properties);

            LinkedList<PropertiesOverride> overrides = null;
            if (_propertiesOverride.TryGetValue(type, out overrides))
            {

                foreach (var propertiesOverride in overrides)
                {
                    propertiesOverride.Set(type, properties);
                }
            }
            return properties.Values.ToList();
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var temp = objectType.GetTypeMeta().Fields.Select(x => x.FieldInfo).Cast<MemberInfo>().ToList();
            return temp;
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