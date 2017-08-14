namespace ArmChair
{
    using System;
    using System.Linq.Expressions;
    using Utils;

    public class IndexEntry
    {
        public IndexEntry()
        {
            Index = new Index();
        }

        /// <summary>
        /// index name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// ddoc to use
        /// </summary>
        public virtual string DesignDocument { get; set; }

        /// <summary>
        /// constains the fields to be index.
        /// </summary>
        public virtual Index Index { get; set; }

        /// <summary>
        /// set a field which will be index'ed
        /// </summary>
        /// <param name="name">the name of the field</param>
        /// <param name="sort">the order of the index</param>
        public virtual void Field(string name, Order sort = Order.Asc)
        {
            Index.Add(name, sort);
        }

        protected internal virtual Type Type { get; set; }

    }

    public class IndexEntry<T> : IndexEntry
    {
        public IndexEntry() : base()
        {
            Type = typeof(T);
        }

        private ReflectionHelper _helper = new ReflectionHelper();

        public IndexEntry<T> Field(Expression<Func<T, object>> property, Order sort = Order.Asc)
        {
            var unary = property.Body as UnaryExpression;
            var exp = unary == null ? (MemberExpression) property.Body : (MemberExpression) unary.Operand;

            string name = _helper.GetMemberName(exp);
            Index.Add(name, sort);
            return this;
        }

    }
}