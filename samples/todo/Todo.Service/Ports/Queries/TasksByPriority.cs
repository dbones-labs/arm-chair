namespace Todo.Service.Ports.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using ArmChair;
    using MediatR;
    using Models;
    
    public class TasksByPriority : IRequest<IEnumerable<Task>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
        public PriorityLevel Priority { get; set; }
    }

    public class TasksByPriorityHandler : IRequestHandler<TasksByPriority, IEnumerable<Task>>
    {
        private readonly ISession _session;

        public TasksByPriorityHandler(ISession session)
        {
            _session = session;
        }
        
        public IEnumerable<Task> Handle(TasksByPriority message)
        {
            var results = _session.Query<Task>()
                .Where(x=> x.Priority == message.Priority)
                .Skip(message.Skip)
                .Take(message.Take)
                .ToList();

            return results;
        }
    }
}