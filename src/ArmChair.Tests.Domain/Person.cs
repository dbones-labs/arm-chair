namespace ArmChair.Tests.Domain
{
    public class Person : EntityRoot
    {
        public Person(string name)
        {
            Name = name;
        }

        protected Person()
        {

        }

        public virtual string Name { get; set; }
    }
}