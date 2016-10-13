namespace ArmChair.Commands
{
    using System.Collections.Generic;

    /// <summary>
    /// the response of running a mongo Query
    /// </summary>
    public class MongoQueryResponse
    {
        public IEnumerable<object> Docs { get; set; }
    }
}