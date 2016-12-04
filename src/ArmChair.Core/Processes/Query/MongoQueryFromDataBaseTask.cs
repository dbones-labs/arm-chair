namespace ArmChair.Processes.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using EntityManagement;
    using IdManagement;
    using Tasks;

    /// <summary>
    /// query the db using the mongo query object
    /// </summary>
    public class MongoQueryFromDataBaseTask : IPipeTask<QueryContext>
    {
        private readonly CouchDb _couchDb;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;


        public MongoQueryFromDataBaseTask(CouchDb couchDb, IIdManager idManager, IIdAccessor idAccessor)
        {
            _couchDb = couchDb;
            _idManager = idManager;
            _idAccessor = idAccessor;
        }

        public IEnumerable<QueryContext> Execute(IEnumerable<QueryContext> items)
        {
            var ctx = items.First();
            var q = ctx.Query;
            var query = new MongoQueryRequest
            {
                Selector = q.Selector,
                Limit = q.Limit,
                Skip = q.Skip,
                Sort = q.Sort
            };

            var allDocs = _couchDb.MongoQuery(query);

            //using the loaded docs, update the context entries.
            foreach (var entity in allDocs.Docs)
            {
                var id = _idAccessor.GetId(entity);
                var key = _idManager.GetFromId(entity.GetType(), id);

                yield return new QueryContext
                {
                    Query = q,
                    Entity = entity,
                    Key = key,
                    Type = entity.GetType()
                };
            }
        }
    }
}