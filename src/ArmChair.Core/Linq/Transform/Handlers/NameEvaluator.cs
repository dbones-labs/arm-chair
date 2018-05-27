namespace ArmChair.Linq.Transform.Handlers
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// this will extract the name an expression.
    /// </summary>
   
    internal class NameEvaluator : ExpressionVisitor
    {
        private readonly VisitorContext _context;
        private bool _started = false;
        private readonly StringBuilder _builder = new StringBuilder();

        public NameEvaluator(VisitorContext context)
        {
            _context = context;
        }

        public string PropertyName => _builder.ToString();

        public override Expression Visit(Expression exp)
        {
            return exp == null ? null : base.Visit(exp);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var result = base.VisitMethodCall(node);
            var isDictionary = typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(node.Method.DeclaringType);

            //only support dictionay index by name
            if (node.Method.Name == "get_Item" && isDictionary)
            {
                if (node.Arguments.Count() > 1) throw new Exception("sorry get_Item (dictionary indexer) is supported, and there seems to be more than one");
                var arg = node.Arguments.FirstOrDefault() as ConstantExpression;
                if (arg == null) throw new Exception("sorry get_Item (dictionary indexer) is supported, and there seems to be no args");
                //Append($"[\"{arg.Value}\"]", false);
                Append($"{arg.Value}");
            }

            return result;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var result = base.VisitMember(node);

            var type = node.Member.DeclaringType;
            var name = node.Member.Name;
            var actualName = GetActualName(type, name);

            Append(actualName);
            return result;
        }

        private void Append(string name, bool dotPrefix = true)
        {
            if (_started && dotPrefix)
            {
                _builder.Append(".");
            }
            else
            {
                _started = true;
            }

            _builder.Append(name);
        }


        private string GetActualName(Type type, string name)
        {
            var idMeta = _context.SessionContext.IdAccessor.GetIdField(type);
            if (idMeta != null && idMeta.FriendlyName == name)
            {
                return "_id";
            }

            return name;
        }
    }


    /// <summary>
    /// this will extract the name an expression.
    /// </summary>
    internal class NameEvaluator2 : ExpressionVisitor
    {
        private readonly VisitorContext _context;
        private readonly QueryObject _right;
        private bool _started = false;
        public QueryObject Query;


        protected NameEvaluator2(VisitorContext context, QueryObject right)
        {
            _context = context;
            _right = right;
        }


        public override Expression Visit(Expression exp)
        {
            return exp == null ? null : base.Visit(exp);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var result = base.VisitMethodCall(node);
            var isDictionary = typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(node.Method.DeclaringType);

            //only support dictionay index by name
            if (node.Method.Name == "get_Item" && isDictionary)
            {
                if (node.Arguments.Count() > 1) throw new Exception("sorry get_Item (dictionary indexer) is supported, and there seems to be more than one");
                var arg = node.Arguments.FirstOrDefault() as ConstantExpression;
                if (arg == null) throw new Exception("sorry get_Item (dictionary indexer) is supported, and there seems to be no args");
                //Append($"[\"{arg.Value}\"]", false);
                Append($"{arg.Value}");
            }

            return result;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var result = base.VisitMember(node);

            var type = node.Member.DeclaringType;
            var name = node.Member.Name;
            var actualName = GetActualName(type, name);

            Append(actualName);
            return result;
        }

        private void Append(string name)
        {
            if (Query == null)
            {
                Query = new QueryObject()
                {
                    { name, _right }
                };
            }
            else
            {
                Query = new QueryObject()
                {
                    { name, Query }
                };
            }
        }


        private string GetActualName(Type type, string name)
        {
            var idMeta = _context.SessionContext.IdAccessor.GetIdField(type);
            if (idMeta != null && idMeta.FriendlyName == name)
            {
                return "_id";
            }

            return name;
        }
    }

}