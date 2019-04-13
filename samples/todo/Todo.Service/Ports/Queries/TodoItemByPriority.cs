namespace Todo.Service.Ports.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ArmChair;
    using MediatR;
    using Models;
    
    public class TasksByPriority : IRequest<IEnumerable<TodoItem>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
        public PriorityLevel Priority { get; set; }
    }

    public class TasksByPriorityHandler : IRequestHandler<TasksByPriority, IEnumerable<TodoItem>>
    {
        private readonly ISession _session;

        public TasksByPriorityHandler(ISession session)
        {
            _session = session;
        }
        
        public Task<IEnumerable<TodoItem>> Handle(TasksByPriority request, CancellationToken cancellationToken)
        {
            IEnumerable<TodoItem> results = _session.Query<TodoItem>()
                .Where(x=> x.Priority == request.Priority)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToList();

            return Task.FromResult(results);
        }

        
    }
}