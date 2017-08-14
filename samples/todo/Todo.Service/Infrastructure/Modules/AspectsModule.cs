namespace Todo.Service.Infrastructure.Modules
{
    using Aspects;
    using Autofac;
    
    public class AspectsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransactionInterceptor>().InstancePerDependency();
        }
    }
}