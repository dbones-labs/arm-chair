namespace Todo.Service.Infrastructure.DbMaps
{
    using ArmChair.EntityManagement.Config;

    public class TaskClassMap : ClassMap<Models.TodoItem>
    {
        public TaskClassMap()
        {
            Id(x => x.Id);
            Revision(x => x.Revision);

            Index(idx => idx.Field(f => f.Priority));
            Index(idx => idx.Field(f => f.Created));
            Index(idx => idx.Field(f => f.LastUpdated));
        }
    }
}
