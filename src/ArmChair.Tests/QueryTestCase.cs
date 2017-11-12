namespace ArmChair.Tests
{
    using System.Linq;
    using System.Reflection;
    using Domain;
    using Domain.Sample1;

    public abstract class QueryTestCase : DataTestCase
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            SetupMaps();
        }

        public virtual void SetupMaps()
        {
            var types = typeof(Person).GetTypeInfo().Assembly.GetTypes().Where(x => typeof(EntityRoot).IsAssignableFrom(x)).ToList();
            Database.Register(types);
        }

    }
}