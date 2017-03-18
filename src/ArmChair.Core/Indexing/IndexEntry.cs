namespace ArmChair
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
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
    }

    public class IndexEntry<T> : IndexEntry
    {
        private ReflectionHelper _helper = new ReflectionHelper();

        public IndexEntry<T> Field(Expression<Func<T, object>> property, Order sort = Order.Asc)
        {
            var unary = property.Body as UnaryExpression;
            var exp = unary == null ? (MemberExpression) property.Body : (MemberExpression) unary.Operand;

            string name = _helper.GetMemberName(exp);
            Index.Add(name, sort);
            return this;
        }

        protected internal virtual void Compile()
        {
            if (string.IsNullOrEmpty(DesignDocument))
            {
                DesignDocument = typeof(T).GetTypeInfo().FullName;
            }
            if (string.IsNullOrEmpty(Name))
            {
                var entries = Index.Fields.Select(x =>
                {
                    var entry = x.First();
                    return $"{entry.Key},{entry.Value}";
                });

                Name = string.Join(",", entries).GetHashCode().ToString();
            }
        }
    }
}