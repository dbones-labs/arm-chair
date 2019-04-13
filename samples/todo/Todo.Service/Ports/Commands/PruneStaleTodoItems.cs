namespace Todo.Service.Ports.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ArmChair;
    using MediatR;

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
        
        public Task<Unit> Handle(PuneStaleTasks request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var todoItems = _mediator.Send(new Queries.TasksByStale()).Result;

            if (todoItems != null)
            {
                _session.RemoveRange(todoItems);
            }
            
            return Task.FromResult(new Unit());
        }

        
    }
}