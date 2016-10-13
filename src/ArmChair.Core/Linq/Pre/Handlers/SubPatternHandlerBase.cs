namespace ArmChair.Linq.Pre.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class SubPatternHandlerBase : ISubPatternHandler
    {
        protected static ReflectionHelper<IEnumerable<object>> ReflectionHelper = new ReflectionHelper<IEnumerable<object>>();
        protected readonly Func<MethodCallExpression, bool> Supported;

        protected SubPatternHandlerBase(Expression<Func<IEnumerable<object>, object>> expression)
        {
            var methodName = ReflectionHelper.GetSupportedMethod(expression).Name;
            Supported = exp => exp.Method.Name.ComparedTo(methodName);
        }

        protected SubPatternHandlerBase(params Expression<Func<IEnumerable<object>, object>>[] expressions)
        {
            List<string> names = expressions.Select(expression => ReflectionHelper.GetSupportedMethod(expression).Name).ToList();

            Supported = exp =>
            {
                var name = exp.Method.Name;
                return names.Any(name.ComparedTo);
            };

        }
        
        public bool CanHandle(ProcessingLinqContext ctx)
        {
            return Supported(ctx.CurrentMethod.Expression);
        }

        public abstract void Update(ProcessingLinqContext ctx);

        public abstract bool IndexQueryCompleted(ProcessingLinqContext ctx);
    }
}