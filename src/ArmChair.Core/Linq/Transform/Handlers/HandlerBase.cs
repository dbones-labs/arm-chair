namespace ArmChair.Linq.Transform.Handlers
{
    using System;
    using System.Linq.Expressions;

    public abstract class HandlerBase<T> : IHandler<T> where T : Expression
    {
        public abstract void Handle(T expression, VisitorContext context);

        public abstract bool CanHandle(T expression);

        public virtual Type HandleTypeOf
        {
            get { return typeof (T); }
        }

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
        protected virtual string GetMemberName(MemberExpression memberExpression)
        {
            var prefixExpression = memberExpression.Expression as MemberExpression;
            if (prefixExpression == null) return memberExpression.Member.Name;

            var prefix = GetMemberName(prefixExpression);
            return string.Join(".", prefix, memberExpression.Member.Name);
        }


    }


}