namespace ArmChair.Tests.Mapping
{
    using EntityManagement.Config;

    public class CustomThingMap : ClassMap<CustomThing>
    {
        public CustomThingMap()
        {
            Id(x => x.EpicId);
            Revision(x => x.IAmARevision);
        }
    }
}