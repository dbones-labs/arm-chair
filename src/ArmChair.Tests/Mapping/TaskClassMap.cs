namespace ArmChair.Tests.Mapping
{
    using Domain.Sample3;
    using EntityManagement.Config;

    public class TaskClassMap : ClassMap<TodoTask>
    {
        public TaskClassMap()
        {
            Id(x => x.Id);
            Revision(x => x.Rev);

            Index(idx => idx.Field(f => f.Priority));
            Index(idx => idx.Field(f => f.Created));
            Index(idx => idx.Field(f => f.LastUpdated));
        }
    }
}