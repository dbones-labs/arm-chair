namespace ArmChair.Linq.Pre
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Handlers;

    public class LinqVisitor : ExpressionVisitor
    {
        private readonly ProcessingLinqContext _ctx;

        private bool _indexQueryDefined = false;
        private readonly IEnumerable<ISubPatternHandler> _handlers;


        private LinqVisitor(ProcessingLinqContext ctx)
        {
            _ctx = ctx;
            _handlers = SubPatternRegistry.Handlers;
        }

        public static LinqQuery Eval(Expression expression)
        {
            ProcessingLinqContext ctx = new ProcessingLinqContext(new LinqQuery(expression));
            var visitor = new LinqVisitor(ctx);
            visitor.Visit(expression);
            return ctx.LinqQuery;
        }


        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            //mainly interested in the left, also handle in reverse order
            var arg = node.Arguments[0];
            Visit(arg);

            //found a subQuery, for couchdb to handle, the rest will be in proc.
            if (_indexQueryDefined || _ctx.PreviousSubPatterns.Any(x => x.IndexQueryCompleted(_ctx)))
            {
                if (_ctx.LinqQuery.ParentQuery == null)
                {
                    _ctx.LinqQuery.ParentQuery = new Query(node, _ctx.LinqQuery.FullQuery);
                }
                return node;
            }

            _ctx.SetCurrentMethod(new Method(node));
            var handlers = _handlers.Where(x => x.CanHandle(_ctx)).ToList();

            //cannot hanle with couchdb
            if (!handlers.Any())
            {
                _ctx.LinqQuery.ParentQuery = new Query(node, _ctx.LinqQuery.FullQuery);
                _indexQueryDefined = true;
                return node;
            }

            //handle the expression
            foreach (var handler in handlers)
            {
                handler.Update(_ctx);
            }

            _ctx.HandledBy(handlers);
            return node;
        }
    }
}
