using System;

namespace ArmChair.Tests.Domain
{
    public class Edition
    {

        public Edition(string name, EditionType editionType)
        {
            Name = name;
            Type = editionType;

        }

        protected Edition()
        {

        }

        public virtual DateTime ReleaseDate { get; set; }
        public virtual string Name { get; private set; }
        public virtual EditionType Type { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as Edition;
            if (other == null)
            {
                return false;
            }

            return other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Type.GetHashCode(); ;
        }
    }
}