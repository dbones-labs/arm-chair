namespace Todo.Service.Ports.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ArmChair;
    using MediatR;
    using Models;
    
    public class TasksByActive : IRequest<IEnumerable<Task>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
        public string OrderBy { get; set; }
    }

    public class TasksByActiveHandler : IRequestHandler<TasksByActive, IEnumerable<Task>>
    {
        private readonly ISession _session;

        public TasksByActiveHandler(ISession session)
        {
            _session = session;
        }
        
        public IEnumerable<Task> Handle(TasksByActive message)
        {
            var query = _session.Query<Task>()
                .Where(x=> x.IsComplete == false);

            if (message.OrderBy == null) message.OrderBy = "date";
            switch (message.OrderBy.ToLower())
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
                
            return query
                .Skip(message.Skip)
                .Take(message.Take)
                .ToList();
        }
    }
}