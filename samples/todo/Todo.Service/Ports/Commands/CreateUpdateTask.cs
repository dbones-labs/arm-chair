namespace Todo.Service.Ports.Commands
{
    using System.ComponentModel.DataAnnotations;
    using ArmChair;
    using MediatR;
    using Models;
    
    public class CreateUpdateTask : IRequest<Task>
    {
        [Required]
        public string Description { get; set; }
        public PriorityLevel PriorityLevel { get; set; } = PriorityLevel.Medium;
    }

    public class CreateUpdateTaskHandler : IRequestHandler<CreateUpdateTask, Task>
    {
        private readonly ISession _session;

        public CreateUpdateTaskHandler(ISession session)
        {
            _session = session;
        }
        
        public Task Handle(CreateUpdateTask message)
        {
            var task = new Task(message.Description, message.PriorityLevel);
            _session.Add(task);
            
            return task;
        }
    }
}