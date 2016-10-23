namespace ArmChair.Linq.Transform.Handlers
{
    using System;
    using System.Linq.Expressions;
    using Utils;

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
        protected virtual string GetMemberName(MemberExpression memberExpression, VisitorContext context)
        {
            var prefixExpression = memberExpression.Expression as MemberExpression;

            //note this should be at the root level
            if (prefixExpression == null)
                return GetActualName(memberExpression.Member.DeclaringType, memberExpression.Member.Name, context);

            //as we are not at the root level, we should not need to see if there is an id field,
            var prefix = GetMemberName(prefixExpression, context);
            return string.Join(".", prefix, memberExpression.Member.Name);
        }

        private string GetActualName(Type type, string name, VisitorContext context)
        {
            var idMeta = context.SessionContext.IdAccessor.GetIdField(type);
            if (idMeta != null && idMeta.FriendlyName == name)
            {
                return "_id";
            }
            return name;
        }


    }


}