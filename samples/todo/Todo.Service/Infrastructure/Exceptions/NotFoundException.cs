namespace Todo.Service.Infrastructure.Exceptions
{
    using System;

    public class NotFoundException : Exception
    {
        public NotFoundException(string type, string id)
        {
            Type = type;
            Id = id;
        }
        
        public string Type { get; }
        public string Id { get; }
    }
}