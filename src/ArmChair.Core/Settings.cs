// Copyright 2014 - dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace ArmChair
{
    using System;
    using Commands;
    using EntityManagement;
    using Http;
    using IdManagement;
    using Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Middleware.Commit;
    using Middleware.Load;
    using Middleware.Query;
    using Serialization;
    using Serialization.Newton;
    using Utils.Hashing;
    using Utils.Logging;

    public class DatabaseBuilder
    {
        Settings _settings = new Settings();
        private Func<IIdAccessor, IRevisionAccessor, ISerializer> _createSerializer;


        public DatabaseBuilder(string databaseName, Connection connection)
        {
            _settings.DatabaseName = databaseName;
            _settings.Connection = connection;
        }

        public static DatabaseBuilder Create(string databaseName, Connection connection)
        {
            return new DatabaseBuilder(databaseName, connection);
        }

        public DatabaseBuilder SetLogger(ILogger logger)
        {
            _settings.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            return this;
        }

        public DatabaseBuilder SetSerializer(Func<IIdAccessor, IRevisionAccessor, ISerializer> createSerializer)
        {
            _createSerializer = createSerializer ?? throw new ArgumentNullException(nameof(createSerializer));
            return this;
        }

        public DatabaseBuilder SetHash(IHash hash)
        {
            _settings.Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            return this;
        }


        public Database Build()
        {
            //global
            _settings.Logger = _settings.Logger ?? new NullLogger();

            _settings.IdAccessor = _settings.IdAccessor ?? new IdAccessor();
            _settings.IdManager = _settings.IdManager ?? new ShortGuidIdManager();
            _settings.RevisionAccessor = _settings.RevisionAccessor ?? new RevisionAccessor();
            _settings.TypeManager = _settings.TypeManager ?? new TypeManager();

            _settings.Serializer = _createSerializer == null
                ? new Serializer(_settings.IdAccessor, _settings.RevisionAccessor)
                : _createSerializer(_settings.IdAccessor, _settings.RevisionAccessor);

            //query serializer
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver =
                    new CamelCasePropertyNamesContractResolver()
                    {
                        NamingStrategy = new LocalCamelCaseNamingStrategy(true)
                    }
            };
            settings.Converters.Add(new OrderEnumConverter());

            _settings.QuerySerializer = _settings.QuerySerializer ?? new Serializer(settings);

            //these are not override-able at the moment.
            _settings.CouchDb = new CouchDb(_settings.DatabaseName, _settings.Connection, _settings.Serializer, _settings.QuerySerializer, _settings.Logger);

            _settings.QueryFactory = new QueryFactory(_settings.TypeManager, _settings.IdAccessor);
            _settings.LoadPipeline = new LoadPipeline(_settings.CouchDb, _settings.IdManager, _settings.IdAccessor, _settings.RevisionAccessor);
            _settings.CommitPipeline = new CommitPipeline(_settings.CouchDb, _settings.IdManager, _settings.RevisionAccessor);
            _settings.QueryPipeline = new QueryPipeline(_settings.CouchDb, _settings.IdManager, _settings.IdAccessor, _settings.RevisionAccessor);

            _settings.Hash = _settings.Hash ?? new ToLowerPassThroughHash();

            return new Database(_settings);
        }

    }





    public class Settings
    {
        public string DatabaseName { get; internal set; }
        public Connection Connection { get; internal set; }

        public ILogger Logger { get; internal set; }

        public IIdAccessor IdAccessor { get; internal set; }
        public IIdManager IdManager { get; internal set; }
        public IRevisionAccessor RevisionAccessor { get; internal set; }
        public ISerializer Serializer { get; internal set; }
        public ISerializer QuerySerializer { get; internal set; }
        public ITypeManager TypeManager { get; internal set; }
        public IHash Hash { get; set; }

        public LoadPipeline LoadPipeline { get; internal set; }
        public CommitPipeline CommitPipeline { get; internal set; }
        public QueryPipeline QueryPipeline { get; internal set; }
        public CouchDb CouchDb { get; internal set; }
        public QueryFactory QueryFactory { get; internal set; }
    }


    internal class Settings2 : Settings
    {
        public Settings2(string databaseName, Connection connection, ILogger logger = null)
        {
            Logger = logger ?? new NullLogger();

            //global
            IdAccessor = new IdAccessor();
            IdManager = new ShortGuidIdManager();
            RevisionAccessor = new RevisionAccessor();
            TypeManager = new TypeManager();

            Serializer = new Serializer(IdAccessor, RevisionAccessor);

            //query serializer
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver =
                    new CamelCasePropertyNamesContractResolver()
                    {
                        NamingStrategy = new LocalCamelCaseNamingStrategy(true)
                    }
            };
            settings.Converters.Add(new OrderEnumConverter());
            QuerySerializer = new Serializer(settings);

            CouchDb = new CouchDb(databaseName, connection, Serializer, QuerySerializer, Logger);

            QueryFactory = new QueryFactory(TypeManager, IdAccessor);

            LoadPipeline = new LoadPipeline(CouchDb, IdManager, IdAccessor, RevisionAccessor);
            CommitPipeline = new CommitPipeline(CouchDb, IdManager, RevisionAccessor);
            QueryPipeline = new QueryPipeline(CouchDb, IdManager, IdAccessor, RevisionAccessor);

            Hash = new ToLowerPassThroughHash();
        }


    }
}