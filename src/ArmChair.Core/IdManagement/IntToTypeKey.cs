using System;

namespace ArmChair.IdManagement
{
    public class IntToTypeKey : Key
    {
        private readonly object _id;
        private readonly int _hash;
        private readonly string _readFriendly;

        public IntToTypeKey(Type type, int id, string couchId)
        {
            //TODO: extract int from the couchdb.

            _id = id;
            _readFriendly = string.Format("{0}/{1}", type.FullName, id);
            _hash = _readFriendly.GetHashCode();
        }

        public override object Id
        {
            get { return _id; }
        }

        public override string CouchDbId
        {
            get { return _readFriendly; }
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