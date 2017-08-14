namespace Todo.Service.Infrastructure.Modules
{
    using System.Collections.Generic;
    using Autofac;
    using System.Reflection;
    using AutoMapper;
    using AutoMapper.Configuration;
    using Module = Autofac.Module;

    public class MappingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //need to look at this again
            //https://lostechies.com/jimmybogard/2016/07/20/integrating-automapper-with-asp-net-core-di/

            //for now the

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => typeof(Profile).IsAssignableFrom(x))
                .Where(x => !x.GetTypeInfo().IsAbstract)
                .AsSelf()
                .As<Profile>()
                .SingleInstance();

            builder.Register(c =>
                {
                    var cfg = new MapperConfigurationExpression();
                    cfg.ConstructServicesUsing(c.Resolve);
                    c.Resolve<IEnumerable<Profile>>().ForEach(cfg.AddProfile);
                    var mc = new MapperConfiguration(cfg);
                    //mc.AssertConfigurationIsValid();
                    return mc;
                })
                .As<IConfigurationProvider>()
                .SingleInstance();

            builder
                .Register(c => new Mapper(c.Resolve<IConfigurationProvider>(), c.Resolve))
                .As<IMapper>();
        }
    }

}