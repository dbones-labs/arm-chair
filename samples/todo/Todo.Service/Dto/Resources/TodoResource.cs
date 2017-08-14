namespace Todo.Service.Dto.Resources
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public class TodoResource : Resource
    {
        [Required]
        public string Description { get; set; }

        public bool IsComplete { get; set; }

        public DateTime Created { get; set; }

        public PriorityLevel Type { get; set; }
    }
}