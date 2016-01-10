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
using System;
using System.Collections.Generic;

namespace ArmChair.Utils.Copying
{
    /// <summary>
    /// this is an implementation of a shawdow copy
    /// </summary>
    public class ShadowCopier
    {
        readonly IDictionary<Type, CopyMeta> _copyMetas = new Dictionary<Type, CopyMeta>();

        public object Copy(object source, object destination = null)
        {
            if (source == null) return null;
            var copyType = source.GetType();
            CopyMeta meta;
            if (!_copyMetas.TryGetValue(copyType, out meta))
            {
                meta = BuildMeta(copyType);
                _copyMetas.Add(copyType, meta);
            }

            return meta.Copy(source, destination);
        }

        private CopyMeta BuildMeta(Type copyType)
        {
            var meta = new CopyMeta(copyType);
            meta.Congfigure(this);
            return meta;
        }

    }
}
