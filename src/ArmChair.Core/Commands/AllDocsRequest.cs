namespace ArmChair.Commands
{
    using System.Collections.Generic;

    public class AllDocsRequest
    {
        public IEnumerable<string> Keys { get; set; }
    }
}