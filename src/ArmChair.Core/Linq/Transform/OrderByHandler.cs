namespace ArmChair.Linq.Transform
{
    using System;
    using System.Linq.Expressions;

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
}