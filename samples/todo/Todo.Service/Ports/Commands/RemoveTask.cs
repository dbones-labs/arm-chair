namespace Todo.Service.Ports.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
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
        
        public void Handle(RemoveTask message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            var task = _session.GetById<Task>(message.Id);

            if (task != null)
            {
                _session.Remove(task);
            }
        }
    }
}