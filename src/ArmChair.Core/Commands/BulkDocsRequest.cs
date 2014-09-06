namespace ArmChair.Commands
{
    using System.Collections.Generic;

    public class BulkDocsRequest
    {
        public IEnumerable<BulkDocRequest> Docs { get; set; }
    }
}