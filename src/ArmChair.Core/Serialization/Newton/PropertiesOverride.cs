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
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// this allows for a classes Json.NET properties to be overridden
    /// consider this as an alternative to converters
    /// </summary>
    public abstract class PropertiesOverride
    {
        /// <summary>
        /// creates a properties override
        /// </summary>
        protected PropertiesOverride()
        {
            Type = GetSupportedType();
        }

        /// <summary>
        /// the type which this override is for
        /// </summary>
        public Type Type { get; protected set; }

        /// <summary>
        /// contains the override logic
        /// </summary>
        /// <param name="actualType">the actual type which is being overridden</param>
        /// <param name="properties">a dictionary of properties, note that the name is used (not the underlying name)</param>
        public abstract void Set(Type actualType, IDictionary<string, JsonProperty> properties);

        /// <summary>
        /// override this to say what type your override is for
        /// </summary>
        /// <returns></returns>
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
                    chArray[index] = Char.ToLower(chArray[index]);
                else
                    break;
            }
            return new string(chArray);
        }
    }

    /// <summary>
    /// this allows for a classes Json.NET properties to be overridden
    /// consider this as an alternative to converters
    /// </summary>
    public abstract class PropertiesOverride<T> : PropertiesOverride where T : class
    {
        protected override Type GetSupportedType()
        {
            return typeof(T);
        }
    }
}