namespace Todo.Service.Infrastructure.Modules
{
    using Autofac;
    using Behaviours;
    using MediatR;

    public class BehaviorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(UnitOfWorkBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}