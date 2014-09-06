namespace ArmChair.IdManagement
{
    public abstract class Key
    {
        public abstract object Id { get; }
        public abstract string CouchDbId { get; }
        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            var otherKey = obj as Key;
            if (otherKey == null)
            {
                return false;
            }
            return otherKey.GetHashCode() == GetHashCode();
        }

        /// <summary>
        /// The string value is used to save to the database.
        /// </summary>
        /// <returns>it should return a fully unique value.</returns>
        public abstract override string ToString();

    }
}