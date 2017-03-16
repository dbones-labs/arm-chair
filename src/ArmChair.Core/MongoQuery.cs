namespace ArmChair
{
    using System.Collections.Generic;

    public class MongoQuery
    {
        /// <summary>
        /// JSON object describing criteria used to select documents. More information provided in the section on selector syntax.
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


        public string Index { get; set; }

        /// <summary>
        /// the sorts to apply to this query, note an index is required to use a sort.
        /// </summary>
        public IList<IDictionary<string, Order>> Sort { get; set; }
    }


    public class IndexEntry
    {
        public IndexEntry()
        {
            Index = new Index();
        }

        public string Name { get; set; }
        public string DesignDocument { get; set; }
        public Index Index { get; set; }
    }

    public class Index
    {
        public Index()
        {
            Fields = new List<IDictionary<string, Order>>();
        }

        public IList<IDictionary<string,Order>> Fields { get; set; }

        public void Add(string name, Order sort)
        {
            Fields.Add(new Dictionary<string, Order>(){
            {
                name, sort
            }});
        }
    }



}