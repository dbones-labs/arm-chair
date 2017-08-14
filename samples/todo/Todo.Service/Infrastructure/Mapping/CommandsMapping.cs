namespace Todo.Service.Infrastructure.Mapping
{
    using Dto.Resources;
    using Ports.Commands;

    public class CommandsMapping : AutoMapper.Profile
    {
        public CommandsMapping()
        {
            CreateMap<TodoResource, CreateUpdateTask>();
            CreateMap<TodoResource, MarkTaskAsComplete>();
        }
    }
}