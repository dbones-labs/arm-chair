namespace ArmChair.Processes.Update
{
    using System.Collections.Generic;

    public class UpdatePayLoad
    {
        public IEnumerable<object> Deletes { get; set; }
        public IEnumerable<object> Updates { get; set; }
        public IEnumerable<object> Creates { get; set; }
    }
}