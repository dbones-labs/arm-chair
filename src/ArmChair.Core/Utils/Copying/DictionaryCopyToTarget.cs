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
namespace ArmChair.Utils.Copying
{
    using System;
    using System.Collections;
    using System.Reflection;

    /// <summary>
    /// handle to copy of a dictionary
    /// </summary>
    public class DictionaryCopyToTarget : CopyToTarget
    {
        private readonly Func<object, object> _getKeyValue;
        private readonly Func<object, object> _getValue;

        public DictionaryCopyToTarget(Type type)
        {
            var genericArguments = type.GetTypeInfo().GetGenericArguments();
            _getKeyValue = genericArguments[0].GetTypeInfo().IsValueType
                ? (Func<object, object>)(source => source)
                : (Func<object, object>)(source => Copier.Copy(source));

            _getValue = genericArguments[1].GetTypeInfo().IsValueType
                ? (Func<object, object>)(source => source)
                : (Func<object, object>)(source => Copier.Copy(source));
        }

        public override void Copy(object source, object destination)
        {
            var srcCollection = (IDictionary)source;
            var destCollection = (IDictionary)destination;

            foreach (DictionaryEntry entry in srcCollection)
            {
                var key = _getKeyValue(entry);
                var value = _getValue(entry);
                destCollection.Add(key, value);
            }
        }
    }
}