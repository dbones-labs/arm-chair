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
        private readonly IIdAccessor _idAccessor;

        public QueryFactory(
            ITypeManager typeManager,
            IIdAccessor idAccessor)
        {
            _typeManager = typeManager;
            _idAccessor = idAccessor;
        }

        public Query<T> Create<T>(ISession session, string index = null) where T : class
        {
            return new Query<T>(new QueryProvider<T>(_typeManager, _idAccessor,  session, index));
        }
    }
}