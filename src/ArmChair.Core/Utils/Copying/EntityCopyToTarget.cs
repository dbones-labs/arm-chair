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

    /// <summary>
    /// copy an entity
    /// </summary>
    public class EntityCopyToTarget : CopyToTarget
    {
        private readonly FieldMeta _field;
        private readonly Action<object, object> _action;

        public EntityCopyToTarget(FieldMeta field)
        {
            _field = field;

            _action = _field.IsReadOnly
                ? (Action<object, object>)((s, d) =>
                {
                    var dest = field.GetFieldValueFor(d);
                    Copier.Copy(s, dest);
                })
                : (Action<object, object>)((s, d) =>
                {
                    var copy = Copier.Copy(s);
                    _field.SetFieldValueOf(d, copy);
                });
        }

        public override void Copy(object source, object destination)
        {
            var value = _field.GetFieldValueFor(source);
            _action(value, destination);

        }
    }
}