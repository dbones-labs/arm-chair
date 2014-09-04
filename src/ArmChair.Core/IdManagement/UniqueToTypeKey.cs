using System;

namespace ArmChair.IdManagement
{
    public class UniqueToTypeKey : Key
    {
        private readonly object _id;
        private readonly int _hash;
        private readonly string _readFriendly;

        public UniqueToTypeKey(Type type, object id)
        {
            _id = id;
            _readFriendly = string.Format("{0}/{1}", type.FullName, id);
            _hash = _readFriendly.GetHashCode();
        }

        public override object Id
        {
            get { return _id; }
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public override string ToString()
        {
            return _readFriendly;
        }
    }
}