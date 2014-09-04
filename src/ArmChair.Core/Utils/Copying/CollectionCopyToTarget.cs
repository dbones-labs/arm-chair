using System;
using System.Collections;

namespace ArmChair.Utils.Copying
{
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