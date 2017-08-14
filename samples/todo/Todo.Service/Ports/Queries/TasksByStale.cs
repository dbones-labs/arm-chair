namespace Todo.Service.Ports.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ArmChair;
    using MediatR;
    using Models;

    public class TasksByStale : IRequest<IEnumerable<Task>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }

    public class TasksByStaleHandler : IRequestHandler<TasksByStale, IEnumerable<Task>>
    {
        private readonly ISession _session;

        public TasksByStaleHandler(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Task> Handle(TasksByStale message)
        {
            var query = _session.Query<Task>()
                .Where(x => x.LastUpdated < DateTime.UtcNow.AddDays(-1))
                .Where(x => x.IsComplete == true);

            return query
                .Skip(message.Skip)
                .Take(message.Take)
                .ToList();
        }
    }
}