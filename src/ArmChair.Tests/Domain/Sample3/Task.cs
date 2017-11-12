namespace ArmChair.Tests.Domain.Sample3
{
    using System;

    public class TodoTask : EntityRoot
    {
        private string _description;
        private bool _isComplete;

        protected TodoTask()
        {

        }

        public TodoTask(string description, PriorityLevel priorityLevel)
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
                LastUpdated = DateTime.UtcNow;
            }
        }

        public PriorityLevel Priority { get; set; }
    }
}
