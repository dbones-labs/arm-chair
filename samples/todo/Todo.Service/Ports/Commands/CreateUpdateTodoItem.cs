namespace Todo.Service.Ports.Commands
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;
    using ArmChair;
    using MediatR;
    using Models;
    
    public class CreateUpdateTask : IRequest<TodoItem>
    {
        [Required]
        public string Description { get; set; }
        public PriorityLevel PriorityLevel { get; set; } = PriorityLevel.Medium;
    }

    public class CreateUpdateTaskHandler : IRequestHandler<CreateUpdateTask, TodoItem>
    {
        private readonly ISession _session;

        public CreateUpdateTaskHandler(ISession session)
        {
            _session = session;
        }
        
        

        public Task<TodoItem> Handle(CreateUpdateTask request, CancellationToken cancellationToken)
        {
            var todoItem = new TodoItem(request.Description, request.PriorityLevel);
            _session.Add(todoItem);
            
            return Task.FromResult(todoItem);  
        }
    }
}