namespace ArmChair.Linq.Transform.Handlers
{
    using System;
    using System.Linq.Expressions;

    public interface IHandler
    {
        Type HandleTypeOf { get; }
        bool CanHandle(Expression expression);
        void Handle(Expression expression, VisitorContext context);
    }

    public interface IHandler<in T> : IHandler where T : Expression
    {
        void Handle(T expression, VisitorContext context);
        bool CanHandle(T expression);
    }
}