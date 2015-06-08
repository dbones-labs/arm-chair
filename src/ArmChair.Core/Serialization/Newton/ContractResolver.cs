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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using EntityManagement;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Utils;

    /// <summary>
    /// Armchair overrides the default contract resolver to add some support for its naming conventions
    /// </summary>
    public class ContractResolver : CamelCasePropertyNamesContractResolver
    {
        private IDictionary<Type, LinkedList<PropertiesOverride>> _propertiesOverride = new Dictionary<Type, LinkedList<PropertiesOverride>>();
        protected PropertiesOverride _baseOverride;

        public ContractResolver(IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            _baseOverride = new BaseOverride(idAccessor, revisionAccessor);
        }

        
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


    public abstract class PropertiesOverride
    {
        protected PropertiesOverride()
        {
            Type = GetSupportedType();
        }

        public Type Type { get; protected set; }

        public abstract void Set(Type actualType, IDictionary<string, JsonProperty> properties);

        protected abstract Type GetSupportedType();

        protected static string ToCamelCase(string s)
        {
            if (String.IsNullOrEmpty(s) || !Char.IsUpper(s[0]))
                return s;
            char[] chArray = s.ToCharArray();
            for (int index = 0; index < chArray.Length; ++index)
            {
                bool flag = index + 1 < chArray.Length;
                if (index <= 0 || !flag || Char.IsUpper(chArray[index + 1]))
                    chArray[index] = Char.ToLower(chArray[index], CultureInfo.InvariantCulture);
                else
                    break;
            }
            return new string(chArray);
        }
    }

    public abstract class PropertiesOverride<T> : PropertiesOverride where T : class
    {
        protected override Type GetSupportedType()
        {
            return typeof (T);
        }
    }

    public class BaseOverride : PropertiesOverride<object>
    {
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        public BaseOverride(IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
        }

        public override void Set(Type actualType, IDictionary<string, JsonProperty> properties)
        {
            var idField = _idAccessor.GetIdField(actualType);
            var revField = _revisionAccessor.GetRevisionField(actualType);

            if (idField == null || revField == null)
            {
                return;
            }

            properties[ToCamelCase(idField.FriendlyName)].PropertyName = "_id";
            properties[ToCamelCase(revField.FriendlyName)].PropertyName = "_rev";
        }
    }

}