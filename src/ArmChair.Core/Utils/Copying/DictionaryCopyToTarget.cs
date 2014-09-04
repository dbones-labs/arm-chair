using System;
using System.Collections;

namespace ArmChair.Utils.Copying
{
    public class DictionaryCopyToTarget : CopyToTarget
    {
        private readonly Func<object, object> _getKeyValue;
        private readonly Func<object, object> _getValue;
        public DictionaryCopyToTarget(Type type)
        {
            var genericArguments = type.GetGenericArguments();
            _getKeyValue = genericArguments[0].IsValueType
                ? (Func<object, object>)(source => source)
                : (Func<object, object>)(source => Copier.Copy(source));

            _getValue = genericArguments[1].IsValueType
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