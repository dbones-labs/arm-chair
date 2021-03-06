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
namespace ArmChair.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Exceptions;
    using Http;
    using Serialization;
    using Utils.Logging;

    /// <summary>
    /// PLEASE USE <see cref="Database"/>. This represents the CouchDB API, allowing execution of supported commands.
    /// </summary>
    /// <remarks>
    /// really want to call this DatabaseApi
    /// </remarks>
    public class CouchDb
    {
        private readonly string _name;
        private readonly IConnection _connection;
        private readonly ISerializer _serializer;
        private readonly ISerializer _querySerializer;
        private readonly ILogger _logger;


        /// <summary>
        /// Create an instance of a CouchDb
        /// </summary>
        /// <param name="name">name of the CouchDb</param>
        /// <param name="connection">the connection to the CouchDb</param>
        /// <param name="serializer">the serializer to use when exxchanging data</param>
        /// <param name="querySerializer">serializer for mongo queries</param>
        /// <param name="logger">a logger which we can log the requests and responses too for debugging only</param>
        public CouchDb(string name, Connection connection, ISerializer serializer, ISerializer querySerializer, ILogger logger)
        {
            _logger = logger;
            _name = name;
            _connection = connection;
            _serializer = serializer;
            _querySerializer = querySerializer;
            _logger = logger;
        }

        /// <summary>
        /// Load a single Document
        /// </summary>
        /// <param name="key">the key/id of the document</param>
        /// <param name="objecType">the type that is being loaded</param>
        /// <returns>returns the document in its POCO form</returns>
        public virtual object LoadEntity(string key, Type objecType)
        {
            var request = new Request("/:db/:key", HttpMethod.Get);
            request.AddUrlSegment("db", _name);
            request.AddUrlSegment("key", key);

            _logger.Log(()=> $"loading {objecType.FullName}:{key}");

            using (var response = _connection.Execute(request))
            {
                if (response.Status == HttpStatusCode.NotFound)
                {
                    _logger.Log(() => $"not found {objecType.FullName}:{key}");

                    return null;
                }

                var content = response.GetBody();
                _logger.Log(() => $"loaded {objecType.FullName}:{key}{Environment.NewLine}{content}");
                return _serializer.Deserialize(content, objecType);
            }
        }

        /// <summary>
        /// Load a number of documents via their keys
        /// </summary>
        /// <param name="keys">keys/ids of the documents to load</param>
        /// <returns>the documents loaded into their assocaited POCO's</returns>
        public virtual AllDocsResponse LoadAllEntities(AllDocsRequest keys)
        {
            var requestJson = _serializer.Serialize(keys);
            var request = new Request("/:db/_all_docs", HttpMethod.Post);
            request.AddUrlSegment("db", _name);
            request.AddParameter("include_docs", "true"); //load entire content.
            request.AddContent(() => new StringContent(requestJson), ContentType.Json);

            _logger.Log(() => $"loading multiple objects: [{requestJson}]");

            using (var response = _connection.Execute(request))
            {
                var content = response.GetBody();
                _logger.Log(() => $"loading multiple objects: [{requestJson}]{Environment.NewLine}{content}");
                return _serializer.Deserialize<AllDocsResponse>(content);
            }
        }

        /// <summary>
        /// apply a number of changes in 1 bulk 
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        public virtual IEnumerable<BulkDocResponse> BulkApplyChanges(BulkDocsRequest updates)
        {
            var json = _serializer.Serialize(updates);
            var request = new Request("/:db/_bulk_docs", HttpMethod.Post);
            request.AddUrlSegment("db", _name);
            request.AddContent(()=> new StringContent(json), ContentType.Json);

            _logger.Log(()=> $"applying changes:{Environment.NewLine}{json}");

            using (var response = _connection.Execute(request))
            {
                var content = response.GetBody();
                _logger.Log(() => $"applyed changes:{Environment.NewLine}{content}");
                var results = _serializer.Deserialize<IEnumerable<BulkDocResponse>>(content);
                results = CheckForExceptions(results);
                return results;
            }
        }

        /// <summary>
        /// executes a mongo query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual MongoQueryResponse MongoQuery(MongoQueryRequest query)
        {
            //use_index 
            var json = _querySerializer.Serialize(query);
            var request = new Request("/:db/_find", HttpMethod.Post);
            request.AddUrlSegment("db", _name);
            request.AddContent(()=> new StringContent(json), ContentType.Json);

            _logger.Log(()=> $"querying:{Environment.NewLine}{json}");

            using (var response = _connection.Execute(request))
            {
                var content = response.GetBody();
                _logger.Log(() => $"query results:{Environment.NewLine}{content}");
                return _serializer.Deserialize<MongoQueryResponse>(content);
            }
        }

        /// <summary>
        /// Creates an index on the server.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual CreateIndexResponse CreateIndex(CreateIndexRequest index)
        {
            var json = _querySerializer.Serialize(index);
            var request = new Request("/:db/_index", HttpMethod.Post);
            request.AddUrlSegment("db", _name);
            request.AddContent(()=> new StringContent(json), ContentType.Json);

            _logger.Log(() => $"creating index:{Environment.NewLine}{json}");

            using (var response = _connection.Execute(request))
            {
                var content = response.GetBody();
                _logger.Log(() => $"index creation response:{Environment.NewLine}{content}");
                return _serializer.Deserialize<CreateIndexResponse>(content);
            }
        }

        private IEnumerable<BulkDocResponse> CheckForExceptions(IEnumerable<BulkDocResponse> results)
        {
            List<CouchDbException> exceptions = new List<CouchDbException>();
            foreach (var result in results)
            {
                if (result.Ok == true)
                {
                    yield return result;
                    continue;
                }
                
                switch (result.Error)
                {
                    case "conflict": 
                        exceptions.Add(new ConflictException(result.Id, result.Rev, result.Error, result.Reason));
                        break;
                    default:
                        exceptions.Add(new CouchDbException(result.Id, result.Rev, result.Error, result.Reason));
                        break;
                }
            }
            
            if(exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}