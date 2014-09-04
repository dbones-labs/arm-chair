namespace ArmChair.IdManagement
{
    public class GlobalKey : Key
    {
        private readonly object _id;
        private readonly int _hash;
        private readonly string _readFriendly;

        public GlobalKey(object id)
        {
            _id = id;
            _hash = _id.GetHashCode();
            _readFriendly = _id.ToString();
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