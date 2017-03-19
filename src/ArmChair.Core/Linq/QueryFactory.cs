namespace ArmChair.Linq
{
    using System.Collections.Generic;
    using EntityManagement;
    using IQToolkit;

    /// <summary>
    /// this will create a linq query object
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

        public Query<T> Create<T>(ISession session, IEnumerable<string> index = null) where T : class
        {
            return new Query<T>(new QueryProvider<T>(_typeManager, _idAccessor,  session, index));
        }
    }
}