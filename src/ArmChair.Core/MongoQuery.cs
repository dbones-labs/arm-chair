namespace ArmChair
{
    using System.Collections.Generic;
    using Linq;

    /// <summary>
    /// a query using the mongo query syntax
    /// </summary>
    public class MongoQuery
    {
        /// <summary>
        /// JSON object, or <see cref="QueryObject"/>, describing criteria used to select documents. More information
        /// provided in the section on selector syntax.
        /// </summary>
        public IDictionary<string, object> Selector { get; set; }

        /// <summary>
        /// Maximum number of results returned. Default is 25. Optional
        /// </summary>
        public long? Limit { get; set; }

        /// <summary>
        /// Skip the first n results, where n is the value specified. Optional
        /// </summary>
        public long? Skip { get; set; }


        /// <summary>
        /// [design document] [index name]
        /// </summary>
        public object Index { get; set; }

        /// <summary>
        /// the sorts to apply to this query, note an index is required to use a sort.
        /// </summary>
        public IList<IDictionary<string, Order>> Sort { get; set; }
    }
}