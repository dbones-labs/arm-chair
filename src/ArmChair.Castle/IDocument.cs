namespace ArmChair
{
    public interface IDocument
    {
        string CouchDbId { get; set; }
        string CouchDbVersion { get; set; }
    }
}
