namespace ArmChair.IdManagement
{
    using Utils;

    public class ShortGuidKey : Key
    {
        private readonly object _id;
        private readonly int _hash;
        private readonly string _readFriendly;

        public ShortGuidKey(ShortGuid id)
        {
            _id = id;
            _readFriendly = _id.ToString();
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