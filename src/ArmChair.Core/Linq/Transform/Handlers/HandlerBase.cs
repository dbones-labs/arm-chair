namespace ArmChair.Linq.Transform.Handlers
{
    using System;
    using System.Linq.Expressions;
    using BinaryHandlers;

    public abstract class HandlerBase<T> : IHandler<T> where T : Expression
    {
        public abstract void Handle(T expression, VisitorContext context);

        public abstract bool CanHandle(T expression);

        public virtual Type HandleTypeOf => typeof (T);

        public virtual bool CanHandle(Expression expression)
        {
            return CanHandle((T)expression);
        }
        

        public virtual void Handle(Expression expression, VisitorContext context)
        {
            Handle((T)expression, context);
        }

        /// <summary>
        /// gets the full name of the field (hello.world)
        /// </summary>
        /// <param name="memberExpression">the expression to pull this from</param>
        /// <returns></returns>
        protected virtual string GetMemberName(Expression memberExpression, VisitorContext context)
        {
            var evaluator = new NameEvaluator(context);
            evaluator.Visit(memberExpression);
            return evaluator.PropertyName;
        }

    }
}