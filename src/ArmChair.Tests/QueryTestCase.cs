namespace ArmChair.Tests
{
    using System.Linq;
    using Domain;
    using Domain.Sample1;
    using EntityManagement.Config;

    public abstract class QueryTestCase : DataTestCase
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            SetupMaps();
        }

        public virtual void SetupMaps()
        {
            var types = typeof(Person).Assembly.GetTypes().Where(x => typeof(EntityRoot).IsAssignableFrom(x)).ToList();
            Database.Settings.Register(types);
        }

    }
}