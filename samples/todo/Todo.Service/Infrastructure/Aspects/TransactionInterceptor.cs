namespace Todo.Service.Infrastructure.Aspects
{
    using System.Collections.Generic;
    using ArmChair;
    using Castle.DynamicProxy;

    public class TransactionInterceptor : IInterceptor
    {
        static HashSet<string> _skip = new HashSet<string>(new [] { "OnActionExecuted", "Dispose", "OnActionExecutionAsync", "OnActionExecuting" });

        private readonly ISession _session;

        public TransactionInterceptor(ISession session)
        {
            _session = session;
        }
        
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (_skip.Contains(invocation.MethodInvocationTarget.Name))
                return;

            _session.Commit();
        }
    }
}