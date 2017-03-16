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
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Http;
    using InSession;
    using Tracking.Shadowing;

    /// <summary>
    /// An instance of a single Database, which you can create sessions with.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Create the instance of the database
        /// </summary>
        /// <param name="databaseName">name of the database</param>
        /// <param name="connection">the connection to the database server</param>
        public Database(string databaseName, Connection connection)
        {
            Settings = new Settings(databaseName, connection);
        }

        /// <summary>
        /// all the settings used with this database
        /// you can access and override any setting
        /// </summary>
        public virtual Settings Settings { get; private set; }

        /// <summary>
        /// create a session with the database, which will allow 
        /// you to interact with it in a Unit Of Work style
        /// </summary>
        /// <returns>active session</returns>
        public virtual ISession CreateSession()
        {
            //session level
            var tracker = new ShadowTrackingProvider();
            var sessionCache = new SessionCache();

            var session = new Session(
                Settings.LoadPipeline,
                Settings.QueryPipeline,
                Settings.CommitPipeline,
                Settings.QueryFactory,
                Settings.IdManager,
                Settings.IdAccessor,
                tracker,
                sessionCache);
            return session;
        }

        public virtual void CreateIndex(IndexEntry index)
        {
            if (index == null) throw new ArgumentNullException(nameof(index));

            var request = new CreateIndexRequest
            {
                Ddoc = index.DesignDocument,
                Name = index.Name,
            };
            foreach (var field in index.Index.Fields)
            {
                var actualEntry = field.First();
                request.Index.Fields.Add(new Dictionary<string, string>()
                {
                    {
                        actualEntry.Key,
                        actualEntry.Value == Order.Asc ? "asc" : "desc"
                    }
                });
            }

            var response = Settings.CouchDb.CreateIndex(request);
        }
    }
}