namespace ArmChair.Linq.Transform
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Utils;

    public class VisitorContext
    {
        private readonly Stack<IDictionary<string, object>> _terms = new Stack<IDictionary<string, object>>(5);
        private readonly MongoQueryTransformVisitor _visitor;
        public SessionContext SessionContext { get; private set; }


        public VisitorContext(MongoQueryTransformVisitor visitor, SessionContext sessionContext)
        {
            _visitor = visitor;
            SessionContext = sessionContext;
        }

        public void Visit(Expression expression)
        {
            _visitor.Visit(expression);
        }


        public IDictionary<string, object> GetResult()
        {
            return _terms.Pop();
        }

        public void SetResult(IDictionary<string,object> query)
        {
            _terms.Push(query);
        }
    }
}