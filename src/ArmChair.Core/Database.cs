﻿// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
    using System.Collections.Generic;
    using Commands;
    using Http;
    using Serialization;

    /// <summary>
    /// Represents the CouchDB, allowing execution of supported commands.
    /// </summary>
    public class Database
    {
        private readonly string _name;
        private readonly IConnection _connection;
        private readonly ISerializer _serializer;


        /// <summary>
        /// Create an instance of a database
        /// </summary>
        /// <param name="name">name of the database</param>
        /// <param name="connection">the connection to the database</param>
        /// <param name="serializer">the serializer to use when exxchanging data</param>
        public Database(string name, Connection connection, ISerializer serializer)
        {
            _name = name;
            _connection = connection;
            _serializer = serializer;
        }

        /// <summary>
        /// Load a single Document
        /// </summary>
        /// <param name="key">the key/id of the document</param>
        /// <returns>returns the document in its POCO form</returns>
        public object LoadEntity(string key)
        {
            object entity = null;
            var request = new Request("/:db/:key", HttpVerbType.Get);
            request.AddUrlSegment("db", _name);
            request.AddUrlSegment("key", key);

            _connection.Execute(request, response =>
            {
                var content = response.Content.ReadToEnd();
                entity = _serializer.Deserialize<object>(content);//would like a betterway
            });
            return entity;
        }

        /// <summary>
        /// Load a number of documents via their keys
        /// </summary>
        /// <param name="keys">keys/ids of the documents to load</param>
        /// <returns>the documents loaded into their assocaited POCO's</returns>
        public AllDocsResponse LoadAllEntities(AllDocsRequest keys)
        {
            var requestJson = _serializer.Serialize(keys);
            var request = new Request("/:db/_all_docs", HttpVerbType.Post);
            request.AddUrlSegment("db", _name);
            request.AddParameter("include_docs", "true"); //load entire content.
            request.AddContent(writer => writer.Write(requestJson), HttpContentType.Json);

            AllDocsResponse result = null;

            _connection.Execute(request, response =>
            {
                var content = response.Content.ReadToEnd();
                result = _serializer.Deserialize<AllDocsResponse>(content);
            });
            return result;
        }

        /// <summary>
        /// apply a number of changes in 1 bulk 
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        public IEnumerable<BulkDocResponse> BulkApplyChanges(BulkDocsRequest updates)
        {

            var json = _serializer.Serialize(updates);

            var request = new Request("/:db/_bulk_docs", HttpVerbType.Post);
            request.AddUrlSegment("db", _name);
            request.AddContent(writer => writer.Write(json), HttpContentType.Json);

            IEnumerable<BulkDocResponse> results = null;
            _connection.Execute(request, response =>
            {
                var content = response.Content.ReadToEnd();
                results = _serializer.Deserialize<IEnumerable<BulkDocResponse>>(content);
            });
         
            return results;
        }
    }
}