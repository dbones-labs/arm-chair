namespace Todo.Service.Infrastructure.Modules
{
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using ArmChair;
    using ArmChair.Http;
    using ArmChair.Processes.Query;
    using ArmChair.Tasks.BySingleItem;
    using Autofac;
    using Configuration;
    using DbMaps;
    using Microsoft.Extensions.Configuration;
    using Module = Autofac.Module;

    public class DataAccessModule : Module
    {
        private readonly IConfigurationRoot _config;

        public DataAccessModule(IConfigurationRoot config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var dbConfig = _config.GetSection("Database").Get<DatabaseConfig>();

            builder
                .RegisterType<Connection>()
                .As<IConnection>()
                .As<Connection>()
                .SingleInstance()
                .WithParameter("baseUrl", dbConfig.ServerUrl)
                .OnActivated(connectionActivated =>
                {
                    EnsureDatabase(connectionActivated.Instance, dbConfig);
                });

            builder
                .RegisterType<Database>()
                .As<Database>()
                .SingleInstance()
                .WithParameter("databaseName", dbConfig.DatabaseName)
                .OnActivated(databaseActivated =>
                {
                    SetupDatabase(databaseActivated.Instance);
                });

            builder
                .Register(c => c.Resolve<Database>().CreateSession())
                .As<ISession>()
                .InstancePerLifetimeScope();

        }

        /// <summary>
        /// ensure the database exists.
        /// </summary>
        /// <param name="conn">the connection which is going to be used</param>
        /// <param name="dbConfig">the database configuration</param>
        protected void EnsureDatabase(Connection conn, DatabaseConfig dbConfig)
        {
            bool needToCreate = false;

            var checkDb = new Request("/:db");
            checkDb.AddUrlSegment("db", dbConfig.DatabaseName);
            checkDb.SetContentType(ContentType.Json);
            using (var response = conn.Execute(checkDb))
            {
                needToCreate = response.Status == HttpStatusCode.NotFound;
            }

            if (!needToCreate) return;

            //create a test db
            var createDb = new Request("/:db", HttpMethod.Put);
            createDb.AddUrlSegment("db", dbConfig.DatabaseName);
            createDb.SetContentType(ContentType.Json);
            using (var response = conn.Execute(createDb))
            {
                Debug.WriteLine(response.Status);
            }
        }

        /// <summary>
        /// setup the database, mainly inform it about any classmaps
        /// </summary>
        /// <param name="database">the database instance</param>
        protected void SetupDatabase(Database database)
        {
            database.Settings.QueryPipeline.SetItemIterator(new ListsItemIterator<QueryContext>());

            //register maps
            database.Register(new[] { new TaskClassMap() });

        }
    }
}