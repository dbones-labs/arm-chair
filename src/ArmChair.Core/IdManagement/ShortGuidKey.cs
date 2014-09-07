namespace ArmChair.IdManagement
{
    public class ShortGuidKey : Key
    {
        private readonly string _id;
        private readonly int _hash;
        private readonly string _readFriendly;

        public ShortGuidKey(string id)
        {
            _id = id;
            _readFriendly = _id;
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