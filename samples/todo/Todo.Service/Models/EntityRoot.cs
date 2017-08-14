namespace Todo.Service.Models
{
    public abstract class EntityRoot 
    {
        public string Id { get; set; }
        public string Revision { get; set; }
    }
}