namespace ArmChair.Linq.Transform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Handlers;
    using Utils;


    public class OrderByHandler
    {
        private readonly SessionContext _context;

        public OrderByHandler(SessionContext context)
        {
            _context = context;
        }


        /// <summary>
        /// gets the full name of the field (hello.world)
        /// </summary>
        /// <param name="memberExpression">the expression to pull this from</param>
        /// <returns></returns>
        public virtual string GetMemberName(MemberExpression memberExpression)
        {
            var prefixExpression = memberExpression.Expression as MemberExpression;

            //note this should be at the root level
            if (prefixExpression == null)
                return GetActualName(memberExpression.Member.DeclaringType, memberExpression.Member.Name);

            //as we are not at the root level, we should not need to see if there is an id field,
            var prefix = GetMemberName(prefixExpression);
            return string.Join(".", prefix, memberExpression.Member.Name);
        }

        private string GetActualName(Type type, string name)
        {
            var idMeta = _context.IdAccessor.GetIdField(type);
            if (idMeta != null && idMeta.FriendlyName == name)
            {
                return "_id";
            }
            return name;
        }

       
    }

    public class MongoQueryTransformVisitor : ExpressionVisitor
    {
        readonly IDictionary<Type, List<IHandler>> _handlers;
        public VisitorContext Context { get; set; }

        public static IDictionary<string,object> Eval(Expression expression, SessionContext sessionContext)
        {
            var visitor = new MongoQueryTransformVisitor();
            var context = new VisitorContext(visitor, sessionContext);
            visitor.Context = context;

            visitor.Visit(expression);

            return context.GetResult();
        }


        public MongoQueryTransformVisitor()
        {
            _handlers = HandlerRegistry.Handlers;
        }

        public override Expression Visit(Expression node)
        {
            return Handle(node, base.Visit);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            return Handle(node, base.VisitBinary);
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            return Handle(node, base.VisitBlock);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            return Handle(node, base.VisitConditional);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return Handle(node, base.VisitConstant);
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            return Handle(node, base.VisitDebugInfo);
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            return Handle(node, base.VisitDefault);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            return Handle(node, base.VisitDynamic);
        }

        protected override Expression VisitExtension(Expression node)
        {
            return Handle(node, base.VisitExtension);
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            return Handle(node, base.VisitGoto);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            return Handle(node, base.VisitIndex);
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            return Handle(node, base.VisitInvocation);
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            return Handle(node, base.VisitLabel);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            return Handle(node, base.VisitListInit);
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            return Handle(node, base.VisitLoop);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            return Handle(node, base.VisitMember);
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            return Handle(node, base.VisitMemberInit);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return Handle(node, base.VisitMethodCall);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            return Handle(node, base.VisitNew);
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            return Handle(node, base.VisitNewArray);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return Handle(node, base.VisitParameter);
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            return Handle(node, base.VisitRuntimeVariables);
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            return Handle(node, base.VisitSwitch);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return Handle(node, base.VisitTypeBinary);
        }

        protected override Expression VisitTry(TryExpression node)
        {
            return Handle(node, base.VisitTry);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return Handle(node, base.VisitUnary);
        }


        protected virtual Expression Handle<TExpression>(TExpression expression, Func<TExpression, Expression> @default) where TExpression : Expression
        {
            List<IHandler> handlers = null;
            if (!_handlers.TryGetValue(typeof(TExpression), out handlers)) return @default(expression);

            var handler = handlers.SingleOrDefault(x => x.CanHandle(expression));
            if (handler == null) return @default(expression);
            
            handler.Handle(expression, Context);
            return expression;
        }
    }
}