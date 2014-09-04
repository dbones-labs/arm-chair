using System.Collections;
using System.Linq;

namespace ArmChair.Utils.Comparing
{
    public class Comparer
    {
        public virtual bool AreEqual(object left, object right)
        {
            if (left == null && right == null) return true;

            //means that right did not == null
            if (left == null) return false;

            //check to see if they are the same object
            if (left == right) return true;
            if (left.Equals(right)) return true;

            var leftType = left.GetType().GetTypeMeta();
            var rightType = left.GetType().GetTypeMeta();

            //must be the sametype!
            if (leftType != rightType) return false;

            //the == and Equals() should have sufficed for a value type
            if (leftType.Type.IsValueType || left is string) return false;

            if (left is IDictionary)
            {
                return CompareDictionary((IDictionary)left, (IDictionary)right);
            }

            if (left is IEnumerable)
            {
                return CompareCollection((IEnumerable)left, (IEnumerable)right);
            }

            return leftType.Fields.All(fieldMeta => AreEqual(fieldMeta.GetFieldValueFor(left), fieldMeta.GetFieldValueFor(right)));
        }

        protected virtual bool CompareDictionary(IDictionary a, IDictionary b)
        {
            if (a.Keys.Count != b.Keys.Count) return false;

            foreach (object key in a.Keys)
            {
                if (!b.Contains(key)) return false;

                bool areEqual = AreEqual(a[key], b[key]);
                if (!areEqual) return false;
            }

            return true;
        }

        protected virtual bool CompareCollection(IEnumerable a, IEnumerable b)
        {
            if (a is ICollection &&
                ((ICollection)a).Count != ((ICollection)b).Count)
            {
                return false;
            }

            IEnumerator aEnumerator = a.GetEnumerator();
            IEnumerator bEnumerator = b.GetEnumerator();

            while (aEnumerator.MoveNext())
            {
                //not the same length (redundant check)
                if (bEnumerator.MoveNext() == false) return false;

                bool areEqual = AreEqual(aEnumerator.Current, bEnumerator.Current);
                if (!areEqual) return false;
            }

            //not the same length
            if (bEnumerator.MoveNext()) return false;

            return true;
        }
    }
}
