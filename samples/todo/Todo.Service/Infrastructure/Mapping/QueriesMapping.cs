namespace Todo.Service.Infrastructure.Mapping
{
    using Adapters.Controllers;
    using Ports.Queries;

    public class QueriesMapping : AutoMapper.Profile
    {
        public QueriesMapping()
        {
            CreateMap<AllQueryString, AllTasks>();
            CreateMap<ByActiveQueryString, TasksByActive>();
            CreateMap<ByPriorityQueryString, TasksByPriority>();
        }
    }
}