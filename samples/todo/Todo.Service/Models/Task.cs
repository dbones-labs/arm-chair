namespace Todo.Service.Models
{
    using System;

    public class Task : EntityRoot
    {
        private string _description;
        private bool _isComplete;

        protected Task()
        {
            
        }

        public Task(string description, PriorityLevel priorityLevel)
        {
            Created = DateTime.UtcNow;
            _description = description;
            Priority = priorityLevel;
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                LastUpdated = DateTime.UtcNow;
            }
        }

        public DateTime Created { get; }
        public DateTime LastUpdated { get; protected set; }

        public bool IsComplete
        {
            get => _isComplete;
            set
            {
                _isComplete = value;
                LastUpdated =DateTime.UtcNow;
            }
        }

        public PriorityLevel Priority { get; set; }
    }
}