using System;
using System.Collections.Generic;

namespace ArmChair.Utils.Copying
{
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
