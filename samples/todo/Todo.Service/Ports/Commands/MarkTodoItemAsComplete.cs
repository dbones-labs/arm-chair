namespace Todo.Service.Ports.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;
    using ArmChair;
    using Infrastructure.Exceptions;
    using MediatR;
    using Models;

    public class MarkTaskAsComplete : IRequest<TodoItem>
    {
        [Required]
        public string Id { get; set; }
    }

    public class MarkTaskAsCompleteHandler : IRequestHandler<MarkTaskAsComplete, TodoItem>
    {
        private readonly ISession _session;

        public MarkTaskAsCompleteHandler(ISession session)
        {
            _session = session;
        }
        
        public Task<TodoItem> Handle(MarkTaskAsComplete request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var todoItem = _session.GetById<TodoItem>(request.Id);

            if (todoItem == null)
            {
                throw new NotFoundException("Task", request.Id);
            }

            todoItem.IsComplete = true;
            return new Task<TodoItem>(() => todoItem);
        }

    }
}