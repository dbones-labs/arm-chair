// // Copyright 2013 - 2014 dbones.co.uk (David Rundle)
// // 
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// // 
// //     http://www.apache.org/licenses/LICENSE-2.0
// // 
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ArmChair.Utils.Copying
{
    public class CopyMeta
    {
        private readonly Type _source;
        private bool _compiled;
        private bool _manuallySetCtor = false;
        private Func<object> _ctor;
        private readonly object _lock = new object();
        private readonly IList<ICopyToTarget> _copyActions = new List<ICopyToTarget>();
        private ShadowCopier _copier;

        public CopyMeta(Type source)
        {
            _source = source;
        }

        public virtual void Congfigure(ShadowCopier copier)
        {
            _copier = copier;
        }

        public virtual void Compile()
        {
            lock (_lock)
            {
                if (_compiled) return; //second check
                var typeMeta = _source.GetTypeMeta();
                if (_ctor == null)
                {
                    _ctor = typeMeta.Ctor;
                }

                //copying a collection of somesort
                if (typeof(IDictionary).IsAssignableFrom(_source))
                {
                    AddTargetToCopy(new DictionaryCopyToTarget(_source));
                    return;
                }

                if (typeof(IList).IsAssignableFrom(_source))
                {
                    AddTargetToCopy(new CollectionCopyToTarget(_source));
                    return;
                }

                //coping an object/entity
                //the fields hold the actual state
                foreach (var field in typeMeta.Fields)
                {
                    var fieldType = field.Type;

                    if (fieldType.IsValueType || fieldType == typeof(string))
                    {
                        AddTargetToCopy(new ValueCopyToTarget(field));
                    }
                    else
                    {
                        AddTargetToCopy(new EntityCopyToTarget(field));
                    }
                }

                _compiled = true;
            }
        }

        private void AddTargetToCopy(ICopyToTarget copyToTarget)
        {
            copyToTarget.Congfigure(_copier);
            _copyActions.Add(copyToTarget);
        }

        public object Copy(object source, object destination = null)
        {
            if (!_compiled) Compile();
            if (source == null) return null;
            object target = destination ?? _ctor();

            foreach (var copyToTarget in _copyActions)
            {
                copyToTarget.Copy(source, target);
            }

            return target;
        }
    }
}