namespace Todo.Service.Ports.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ArmChair;
    using MediatR;
    using Models;

    public class TasksByStale : IRequest<IEnumerable<TodoItem>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }

    public class TasksByStaleHandler : IRequestHandler<TasksByStale, IEnumerable<TodoItem>>
    {
        private readonly ISession _session;

        public TasksByStaleHandler(ISession session)
        {
            _session = session;
        }

        public Task<IEnumerable<TodoItem>> Handle(TasksByStale request, CancellationToken cancellationToken)
        {
            IEnumerable<TodoItem> results = _session.Query<TodoItem>()
                .Where(x => x.LastUpdated < DateTime.UtcNow.AddDays(-1))
                .Where(x => x.IsComplete)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToList();

            return Task.FromResult(results);
        }

        
    }
}