namespace ArmChair.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using IQToolkit;
    using Pre;
    using Transform;
    using Utils;

    /// <summary>
    /// Linq-Couchdb over Mongo Query
    /// </summary>
    /// <remarks> initial look into supporting Linq-Couchdb over Mongo Query</remarks>
    /// <typeparam name="T">the type which this search is against</typeparam>
    public class QueryProvider<T> : QueryProvider where T: class
    {
        private readonly Session _session;

        public QueryProvider(Session session)
        {
            _session = session;
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

            IDictionary<string, object> query;
            var requiresAnd = linqQuery.WhereClauses.Count() > 1;

            if (!requiresAnd)
            {
                var where = linqQuery.WhereClauses.First();
                query = MongoQueryTransformVisitor.Eval(where, _session);
            }
            else
            {
                var clauses = new List<IDictionary<string, object>>();
                foreach (var whereClause in linqQuery.WhereClauses)
                {
                    var partical = MongoQueryTransformVisitor.Eval(whereClause, _session);
                    clauses.Add(partical);
                }

                query = new QueryObject()
                {
                    { "$and", clauses } 
                };
            }
            
            var mongoQuery = new MongoQuery()
            {
                Selector = query,
                Skip = linqQuery.Paging.Skip,
                Limit = linqQuery.Paging.Take,
                //Sort = linqQuery.Ordering.Select(x=> )
            };

            var collection = _session.Query<T>(mongoQuery);
            if (linqQuery.ParentQuery == null)
            {
                return collection;
            }

            //we have only run some of the query, this will setup the rest
            linqQuery.ParentQuery.RewriteSource(collection);
            var exp = linqQuery.ParentQuery.Expression;
            
            //run the expression!
            var result = Expression.Lambda(exp).Compile().DynamicInvoke();
            return result;
        }

        
    }

    public class QueryObject : Dynamic
    {
        
    }

}