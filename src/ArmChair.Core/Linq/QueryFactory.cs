namespace ArmChair.Linq
{
    using EntityManagement;
    using IQToolkit;

    /// <summary>
    /// 
    /// </summary>
    public class QueryFactory
    {
        private readonly ITypeManager _typeManager;

        public QueryFactory(
            ITypeManager typeManager)
        {
            _typeManager = typeManager;
        }

        public Query<T> Create<T>(ISession session, string index = null) where T : class
        {
            return new Query<T>(new QueryProvider<T>(_typeManager, session, index));
        }
    }
}