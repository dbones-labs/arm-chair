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
namespace ArmChair.Utils.Copying
{
    using System;
    using System.Collections;

    /// <summary>
    /// copy over a collection
    /// </summary>
    public class CollectionCopyToTarget : CopyToTarget
    {
        private readonly Func<object, object> _getValue;

        public CollectionCopyToTarget(Type type)
        {
            var genericArguments = type.GetGenericArguments();
            _getValue = genericArguments[0].IsValueType
                ? (Func<object, object>)(source => source)
                : (Func<object, object>)(source => Copier.Copy(source));
        }

        public override void Copy(object source, object destination)
        {
            var srcCollection = (IList)source;
            var destCollection = (IList)destination;

            foreach (var entry in srcCollection)
            {
                var copy = _getValue(entry);
                destCollection.Add(copy);
            }
        }
    }
}