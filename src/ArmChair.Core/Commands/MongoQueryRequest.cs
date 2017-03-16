namespace ArmChair.Commands
{
    using System.Collections.Generic;
    using Newtonsoft.Json; //TODO: remove attributes which tie this class to newtonsoft

    /// <summary>
    /// a mongo query request.
    /// </summary>
    public class MongoQueryRequest
    {
        /// <summary>
        /// JSON object describing criteria used to select documents. More information provided in the section on selector syntax.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public IDictionary<string, object> Selector { get; set; }

        /// <summary>
        /// Maximum number of results returned. Default is 25. Optional
        /// </summary>
        public long? Limit { get; set; }

        /// <summary>
        /// Skip the first ‘n’ results, where ‘n’ is the value specified. Optional
        /// </summary>
        public long? Skip { get; set; }

        /// <summary>
        /// list of fields to return.
        /// </summary>
        public IList<string> Fields { get; set; }


        /// <summary>
        /// apply sorting to the query, plesae ensure there is an index which can be used.
        /// </summary>
        public IList<IDictionary<string, Order>> Sort { get; set; }
    }
}