namespace ArmChair.Tests.Mapping
{
    using Domain.Sample3;
    using EntityManagement.Config;

    public class TodoMap : ClassMap<TodoTask>
    {
        public TodoMap()
        {
            Id(x=> x.Id);

            Index(idx => idx.Field(x => x.Priority));
            Index(idx => idx.Field(x => x.Created));
        }
    }
}