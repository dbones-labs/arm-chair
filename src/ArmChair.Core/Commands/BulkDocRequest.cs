namespace ArmChair.Commands
{
    public class BulkDocRequest
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public bool Delete { get; set; }
    }
}