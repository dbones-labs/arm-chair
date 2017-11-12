namespace Todo.Service.Infrastructure
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Modules;
    using Newtonsoft.Json;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; private set; }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the Configure method, below.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add services to the collection.
            //http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                opts.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            }); //.AddControllersAsServices();

            services.AddMediatR();
            //services.AddAutoMapper(setup=> {}, DependencyContext.Default);

            services.AddCors(options => options.AddPolicy("AllowAllOrigins",
                policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin();
                }));
            
            // Create the container builder.
            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterInstance(Configuration).As<IConfigurationRoot>();

            builder.RegisterModule<MappingModule>();
            builder.RegisterModule(new DataAccessModule(Configuration));
            builder.RegisterModule<BehaviorModule>();
            builder.RegisterModule<ControllerModule>();

            ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // Configure is where you add middleware. This is called after
        // ConfigureServices. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(
          IApplicationBuilder app,
          ILoggerFactory loggerFactory,
          IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseMvc();

            //clean up the container on app stop.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
