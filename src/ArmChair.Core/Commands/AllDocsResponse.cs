namespace ArmChair.Commands
{
    using System.Collections.Generic;

    public class AllDocsResponse
    {
        public int Offset { get; set; }
        public int TotalRows { get; set; }
        public IEnumerable<AllDocsRowResponse> Rows { get; set; }
    }
}