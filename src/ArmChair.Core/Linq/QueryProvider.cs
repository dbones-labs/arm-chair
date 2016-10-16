namespace ArmChair.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using EntityManagement;
    using IQToolkit;
    using Pre;
    using Transform;

    /// <summary>
    /// Linq-Couchdb over Mongo Query
    /// </summary>
    /// <remarks> initial look into supporting Linq-Couchdb over Mongo Query</remarks>
    /// <typeparam name="T">the type which this search is against</typeparam>
    public class QueryProvider<T> : QueryProvider where T : class
    {
        private readonly SessionContext _sessionContext;
        private readonly ISession _session;
        private readonly string _index;

        public QueryProvider(ITypeManager typeManager, ISession session, string index = null)
        {
            _sessionContext = new SessionContext()
            {
                TypeManager = typeManager
            };
            _session = session;
            _index = index;
        }

        public override string GetQueryText(Expression expression)
        {
            return expression.ToString();
        }

        public override object Execute(Expression expression)
        {
            //preprocess
            expression = PartialEvaluator.Eval(expression);
            var linqQuery = LinqVisitor.Eval(expression);

            //get the type constraint
            var types = _sessionContext.TypeManager.Implementation(typeof(T)).Select(x => $"{x.FullName}, {x.Assembly.GetName().Name}");
            var typesQuery = new QueryObject { { "\\$type", new QueryObject { { "$in", types } } } };

            var clauses = new List<IDictionary<string, object>>();
            clauses.Add(typesQuery);

            //get all the other where clauses
            foreach (var whereClause in linqQuery.WhereClauses)
            {
                var partial = MongoQueryTransformVisitor.Eval(whereClause, _sessionContext);
                clauses.Add(partial);
            }
            
            //either take the single where clause or "and" them all together
            IDictionary<string, object> query;
            if (clauses.Count == 1)
            {
                query = clauses.First();
            }
            else
            {
                query = new QueryObject()
                {
                    { "$and", clauses }
                };
            }

            var mongoQuery = new MongoQuery()
            {
                Index = _index,
                Selector = query,
                Skip = linqQuery.Paging.Skip,
                Limit = linqQuery.Paging.Take,
                //Sort = linqQuery.Ordering.Select(x=> )
            };

            var collection = _session.Query<T>(mongoQuery);
            if (linqQuery.ParentQuery == null)
            {
                return linqQuery.PostProcess.Execute(collection);
            }

            //we have only run some of the query, this will setup the rest
            linqQuery.ParentQuery.RewriteSource(collection);
            var exp = linqQuery.ParentQuery.Expression;

            //run the expression!
            var result = Expression.Lambda(exp).Compile().DynamicInvoke();
            return result;
        }


    }
}