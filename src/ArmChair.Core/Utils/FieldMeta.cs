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
namespace ArmChair.Utils
{
    using System;
    using System.Reflection;

    /// <summary>
    /// stores some helpful meta data about a field. which should speed up the app
    /// </summary>
    public class FieldMeta
    {
        private readonly CallGet _getter;
        private readonly CallSet _setter;
       
        /// <summary>
        /// create an instance of the field meta
        /// </summary>
        /// <param name="fieldInfo">the fieldinfo to analyse</param>
        public FieldMeta(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
            _getter = fieldInfo.CreateFieldGet();

            Name = fieldInfo.Name;
            Type = fieldInfo.FieldType;
            IsReadOnly = fieldInfo.IsInitOnly;

            if (Name.StartsWith("<", StringComparison.CurrentCultureIgnoreCase))
            {
                int index = Name.IndexOf(">", StringComparison.CurrentCultureIgnoreCase);
                FriendlyName = Name.Substring(1, index - 1);
            }

            if (Name.StartsWith("_", StringComparison.CurrentCultureIgnoreCase))
            {
                FriendlyName = Name.Substring(1, Name.Length - 1);
            }

            //if (!fieldInfo.IsInitOnly)
            //{
                _setter = fieldInfo.CreateFieldSet();
            //}
        }

        /// <summary>
        /// the type of the field
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// denotes if the field is readonly
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// name of the field, within the parent class
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// the name of the field without the scoping meta, ie _ or the backing angle brackets
        /// </summary>
        public string FriendlyName { get; private set; }

        /// <summary>
        /// the actual field meta
        /// </summary>
        public FieldInfo FieldInfo { get; private set; }

        /// <summary>
        /// get the value for the field, use this instead of reflection, as it has a compiled delegate for a little more speed.
        /// </summary>
        /// <param name="instance">the class which contains the field</param>
        /// <returns>the value the field</returns>
        public object GetFieldValueFor(object instance)
        {
            return _getter(instance);
        }

        /// <summary>
        /// set the value of the field, this will used a compiled delegate for speed
        /// </summary>
        /// <param name="instance">the class which contains the field</param>
        /// <param name="value">the new vaule</param>
        public void SetFieldValueOf(object instance, object value)
        {
            _setter(instance, value);
        }
    }
}