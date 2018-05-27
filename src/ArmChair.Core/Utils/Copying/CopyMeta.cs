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
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// stores infomation about copying
    /// </summary>
    public class CopyMeta
    {
        private readonly Type _source;
        private readonly TypeInfo _sourceInfo;
        private bool _compiled;
        private Func<object> _ctor;
        private readonly object _lock = new object();
        private readonly IList<ICopyToTarget> _copyActions = new List<ICopyToTarget>();
        private ShadowCopier _copier;

        /// <summary>
        /// create a copy meta for this type
        /// </summary>
        /// <param name="source">the source type</param>
        public CopyMeta(Type source)
        {
            _source = source;
            _sourceInfo = _source.GetTypeInfo();
        }

        /// <summary>
        /// configure this copymeta with the main copier which will run the overall process
        /// </summary>
        /// <param name="copier"></param>
        public virtual void Congfigure(ShadowCopier copier)
        {
            _copier = copier;
        }

        /// <summary>
        /// compiles the copy over to use with each field, so when we do copy it will just run though this compiled list.
        /// </summary>
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

                //its still null
                if (_ctor == null)
                {
                    throw new Exception($"confirm that type {typeMeta.Type.FullName} has a parameterless constructor");
                }

                //copying a collection of somesort
                if (typeof(IDictionary).GetTypeInfo().IsAssignableFrom(_sourceInfo))
                {
                    AddTargetToCopy(new DictionaryCopyToTarget(_source));
                    _compiled = true;
                    return;
                }

                if (typeof(IList).GetTypeInfo().IsAssignableFrom(_sourceInfo))
                {
                    AddTargetToCopy(new CollectionCopyToTarget(_source));
                    _compiled = true;
                    return;
                }

                //coping an object/entity
                //the fields hold the actual state
                foreach (var field in typeMeta.Fields)
                {
                    var fieldType = field.Type;

                    if (fieldType.GetTypeInfo().IsValueType || fieldType == typeof(string))
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

        /// <summary>
        /// add a copy strategy to be ran against this type
        /// </summary>
        /// <param name="copyToTarget"></param>
        private void AddTargetToCopy(ICopyToTarget copyToTarget)
        {
            copyToTarget.Congfigure(_copier);
            _copyActions.Add(copyToTarget);
        }

        /// <summary>
        /// copy over the values from src to dest
        /// </summary>
        /// <param name="source">the src</param>
        /// <param name="destination">can be null, will newup a new instance</param>
        /// <returns>the destination</returns>
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