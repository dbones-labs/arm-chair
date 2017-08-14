namespace Todo.Service.Ports.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ArmChair;
    using MediatR;
    using Models;

    public class PuneStaleTasks : IRequest
    {
    }
    
    public class PruneStaleTasksHandler : IRequestHandler<PuneStaleTasks>
    {
        private readonly IMediator _mediator;
        private readonly ISession _session;

        public PruneStaleTasksHandler(IMediator mediator, ISession session)
        {
            _mediator = mediator;
            _session = session;
        }
        
        public void Handle(PuneStaleTasks message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var tasks = _mediator.Send(new Queries.TasksByStale()).Result;

            if (tasks != null)
            {
                _session.RemoveRange(tasks);
            }
        }
    }
}