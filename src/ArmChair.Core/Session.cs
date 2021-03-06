﻿// Copyright 2014 - dbones.co.uk (David Rundle)
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using EntityManagement;
    using IdManagement;
    using InSession;
    using Linq;
    using Middleware.Commit;
    using Middleware.Load;
    using Middleware.Query;
    using Tracking;

    public class Session : ISession
    {
        private readonly LoadPipeline _loadPipeline;
        private readonly QueryPipeline _queryPipeline;
        private readonly CommitPipeline _commitPipeline;
        private readonly QueryFactory _queryFactory;
        private readonly IIdManager _idManager;
        private readonly IIdAccessor _idAccessor;
        private readonly ITypeManager _typeManager;
        private readonly ITrackingProvider _tracking;
        private readonly ISessionCache _sessionCache;

        public Session(
            LoadPipeline loadPipeline,
            QueryPipeline queryPipeline,
            CommitPipeline commitPipeline,
            QueryFactory queryFactory,
            IIdManager idManager,
            IIdAccessor idAccessor,
            ITrackingProvider tracking,
            ISessionCache sessionCache
        )
        {
            _sessionCache = sessionCache;
            _loadPipeline = loadPipeline;
            _queryPipeline = queryPipeline;
            _commitPipeline = commitPipeline;
            _queryFactory = queryFactory;
            _idManager = idManager;
            _idAccessor = idAccessor;
            _tracking = tracking;
        }

        public virtual void Dispose()
        {
            //do nothing for the moment.
        }

        public virtual void Add<T>(T instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            var type = typeof(T);
            var value = _idAccessor.GetId(instance);
            var key = _idManager.GetFromId(typeof(T), value);

            if (key == null)
            {
                key = _idManager.GenerateId(type);
                _idAccessor.SetId(instance, key.Id);
            }

            var entry = new SessionEntry()
            {
                Action = ActionType.Add,
                Instance = instance,
                Key = key
            };

            _sessionCache.Attach(entry);
        }

        public virtual void Attach<T>(T instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            //allows for attaching exiting objects to this session! 
            //(issue could arise with the rev)
            var value = _idAccessor.GetId(instance);
            var key = _idManager.GetFromId(typeof(T), value);
            var entry = new SessionEntry()
            {
                Action = ActionType.Update,
                Instance = _tracking.TrackInstance(instance),
                Key = key
            };


            _sessionCache.Attach(entry);
        }

        public virtual void Remove<T>(T instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            var value = _idAccessor.GetId(instance);
            var key = _idManager.GetFromId(typeof(T), value);
            var entry = new SessionEntry()
            {
                Action = ActionType.Delete,
                Instance = instance,
                Key = key
            };

            _sessionCache.Attach(entry);
        }

        public virtual IEnumerable<T> GetByIds<T>(IEnumerable ids) where T : class
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));
            if (!ids.GetEnumerator().MoveNext()) return new List<T>();
            var results = _loadPipeline.LoadMany<T>(ids, _sessionCache, _tracking).Result;
            return results;
        }

        public virtual T GetById<T>(object id) where T : class
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            var result = _loadPipeline.LoadOne<T>(id, _sessionCache, _tracking).Result;
            return result;
        }

        public virtual void Commit()
        {
            _commitPipeline.Process(_sessionCache, _tracking).Wait();
        }

        public virtual IEnumerable<T> Query<T>(MongoQuery query) where T : class
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var result = _queryPipeline.Query<T>(query, _sessionCache, _tracking).Result;
            return result;
        }

        public virtual IQueryable<T> Query<T>(string ddoc = null, string index = null) where T : class
        {
            List<string> idx = null;
            if (!string.IsNullOrEmpty(ddoc))
            {
                idx = new List<string>();
                idx.Add(ddoc);

                if (!string.IsNullOrEmpty(index))
                {
                    idx.Add(index);
                }
            }

            return _queryFactory.Create<T>(this, idx);
        }
    }
}