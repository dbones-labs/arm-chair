namespace Todo.Service.Infrastructure.Modules
{
    using System.Diagnostics;
    using Autofac;
    using Microsoft.AspNetCore.Mvc;
    using System.Reflection;
    using Aspects;
    using Autofac.Extras.DynamicProxy;
    using Module = Autofac.Module;
    
    public class ControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => typeof(Controller).IsAssignableFrom(x))
                .AsSelf()
                .As<Controller>()
                .As<ControllerBase>()
                .InstancePerDependency()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor))
                .OnActivating(t =>
                {
                    Debug.Write("asdsad");
                });
        }
    }
}