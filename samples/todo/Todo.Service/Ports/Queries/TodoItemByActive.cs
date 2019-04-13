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
    
    public class TasksByActive : IRequest<IEnumerable<TodoItem>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
        public string OrderBy { get; set; }
    }

    public class TasksByActiveHandler : IRequestHandler<TasksByActive, IEnumerable<TodoItem>>
    {
        private readonly ISession _session;

        public TasksByActiveHandler(ISession session)
        {
            _session = session;
        }
        
        public Task<IEnumerable<TodoItem>> Handle(TasksByActive request, CancellationToken cancellationToken)
        {
            var query = _session.Query<TodoItem>()
                .Where(x=> x.IsComplete == false);

            if (request.OrderBy == null) request.OrderBy = "date";
            switch (request.OrderBy.ToLower())
            {
                case "priority":
                    query = query.OrderByDescending(x => x.Priority);
                    break;
                case "date":
                    query = query.OrderByDescending(x => x.Created);
                    break;
                default:
                    throw new NotSupportedException();
            }
                
            IEnumerable<TodoItem> result = query
                .Skip(request.Skip)
                .Take(request.Take)
                .ToList();

            return Task.FromResult(result);
        }

        
    }
}