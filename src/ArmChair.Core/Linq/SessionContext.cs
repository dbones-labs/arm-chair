namespace ArmChair.Linq
{
    using EntityManagement;

    /// <summary>
    /// how we can pass services around the querying process
    /// </summary>
    /// <remarks>
    /// would be cool to find a better way
    /// </remarks>
    public class SessionContext
    {
        public ITypeManager TypeManager { get; set; }
        public IIdAccessor IdAccessor { get; set; }

        public QueryPart QueryPart { get; set; }
    }


    public enum QueryPart
    {
        Where,
        OrderBy
    }
}