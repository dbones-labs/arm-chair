namespace Todo.Service.Ports.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ArmChair;
    using Infrastructure.Exceptions;
    using MediatR;
    using Models;

    public class MarkTaskAsComplete : IRequest<Task>
    {
        [Required]
        public string Id { get; set; }
    }

    public class MarkTaskAsCompleteHandler : IRequestHandler<MarkTaskAsComplete, Task>
    {
        private readonly ISession _session;

        public MarkTaskAsCompleteHandler(ISession session)
        {
            _session = session;
        }
        
        public Task Handle(MarkTaskAsComplete message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var task = _session.GetById<Task>(message.Id);

            if (task == null)
            {
                throw new NotFoundException("Task", message.Id);
            }

            task.IsComplete = true;
            return task;
        }
    }
}