namespace Todo.Service.Infrastructure.Mapping
{
    using Adapters.Controllers;
    using Ports.Queries;

    public class QueriesMapping : AutoMapper.Profile
    {
        public QueriesMapping()
        {
            CreateMap<AllQueryString, AllTodoItems>();
            CreateMap<ByActiveQueryString, TasksByActive>();
            CreateMap<ByPriorityQueryString, TasksByPriority>();
        }
    }
}