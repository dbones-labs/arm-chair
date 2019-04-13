namespace Todo.Service.Ports.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;
    using ArmChair;
    using MediatR;
    using Models;

    public class RemoveTask : IRequest
    {
        [Required]
        public string Id { get; set; }    
    }
    
    public class RemoveTaskHandler : IRequestHandler<RemoveTask>
    {
        private readonly ISession _session;

        public RemoveTaskHandler(ISession session)
        {
            _session = session;
        }
        
        public Task<Unit> Handle(RemoveTask request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var todoItem = _session.GetById<TodoItem>(request.Id);

            if (todoItem != null)
            {
                _session.Remove(todoItem);
            }

            return Task.FromResult(new Unit());
        }
    }
}