namespace ArmChair.Linq.Pre
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// a view of the query that we are processing
    /// </summary>
    public class LinqQuery
    {
        private readonly List<Expression> _whereClauses = new List<Expression>();
        private readonly List<OrderBy> _ordering = new List<OrderBy>(); 

        public LinqQuery(Expression fullQuery)
        {
            Paging = new Paging();
            FullQuery = fullQuery;
        }

        /// <summary>
        /// the where clasuses to be processsed by couchdb, which needs to be converted into mongo json and executed
        /// </summary>
        public IEnumerable<Expression> WhereClauses { get { return _whereClauses; } }

        /// <summary>
        /// the parent query, any thing we cannot do with couchdb will need to be
        /// executed in .NET with the result set.
        /// </summary>
        public Query ParentQuery { get; set; }

        /// <summary>
        /// the origonal query
        /// </summary>
        public Expression FullQuery { get; set; }

        /// <summary>
        /// simple paging rules
        /// </summary>
        public Paging Paging { get; set; }

        /// <summary>
        /// the property name to order the results by
        /// </summary>
        public IEnumerable<OrderBy> Ordering { get { return _ordering; } }


        public Func<object, object> PostIndexProcessing { get; set; }


        public void AddWhereClause(Expression expression)
        {
            _whereClauses.Add(expression);
        }

        public void AddOrderBy(OrderBy order)
        {
            _ordering.Add(order);
        }
     
        //need to see how we support facets
        //public MethodCallExpression GroupBy { get; set; }

    }
}