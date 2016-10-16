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
    }
}