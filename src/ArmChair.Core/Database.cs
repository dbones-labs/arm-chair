namespace ArmChair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters;
    using EntityManagement;
    using Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Processes.Update;

    public class Database
    {
        private readonly string _name;
        private readonly IConnection _connection;
        private readonly ISerializer _serializer;
        private readonly IRevisionAccessor _revisionAccessor;
        private readonly JsonSerializerSettings _settings;

        public Database(string name, Connection connection, ISerializer serializer)
        {
            _name = name;
            _connection = connection;
            _serializer = serializer;


            //_settings = new JsonSerializerSettings();
            //_settings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
            //_settings.TypeNameHandling = TypeNameHandling.Auto;
            //_settings.ContractResolver = new ContractResolver();
            //_serializer = JsonSerializer.Create(_settings);
        }


        public object LoadEntity(string key)
        {
            object entity = null;
            var request = new Request("/:db/:key", HttpVerbType.Get);
            request.AddUrlSegment("db", _name);
            request.AddUrlSegment("key", key);

            _connection.Execute(request, response =>
            {
                var content = response.Content.ReadToEnd();
                entity = _serializer.Deserialize(content);
            });
            return entity;
        }

        public IEnumerable<object> LoadAllEntities(IEnumerable<string> keys)
        {
            var requestJson = _serializer.Serialize(new AllDocsRequest {Keys = keys});
            IEnumerable<object> entities = null;

            var request = new Request("/:db/_all_docs", HttpVerbType.Post);
            request.AddUrlSegment("db", _name);
            request.AddParameter("include_docs", "true"); //load entire content.
            request.AddContent(writer => writer.Write(requestJson), HttpContentType.Json);

            _connection.Execute(request, response =>
            {
                var content = response.Content.ReadToEnd();
                var result = _serializer.Deserialize<AllDocsResponse>(content);
                entities = result.Rows.Select(x => x.Doc);

                
            });
            return entities;
        }

        public IEnumerable<CommitStatus> BulkApplyChanges(UpdatePayLoad updates)
        {
            //TODO: TOO MUCH going on here
            var docs = new List<JObject>();
            foreach (var entity in updates.Creates.Union(updates.Updates))
            {
                JObject doc = null;
                using (var writer = new JTokenWriter())
                {
                    _serializer.Serialize(writer, entity);
                    writer.Flush();
                    doc = (JObject)writer.Token;
                }
                //TODO: serialization
                //ensure we strip out rev on updates
                var revField = _revisionAccessor.GetRevisionField(entity.GetType());
                if (string.IsNullOrWhiteSpace((string)_revisionAccessor.GetRevision(entity)))
                {
                    doc.Remove("_rev");
                    doc.Remove("rev");
                }
                else
                {
                    doc.Remove("_rev");
                    doc.Remove("rev");
                    doc.Add("_rev", new JValue(_revisionAccessor.GetRevision(entity)));
                }

                var val = (string)doc["id"];
                doc.Remove("id");
                doc.Add("_id", new JValue(val));

                docs.Add(doc);
            }
            foreach (var entity in updates.Deletes)
            {
                //Need to make faser!?
                JObject temp = null;
                using (var writer = new JTokenWriter(temp))
                {
                    _serializer.Serialize(writer, entity);
                    writer.Flush();
                    temp = (JObject)writer.Token;
                }

                var requiredForDelete = temp.Properties().Where(x => x.Name == "_id" || x.Name == "_rev");
                var doc = new JObject(requiredForDelete);

                docs.Add(doc);
            }

            var payload = new JObject();
            payload.Add("all_or_nothing", new JValue(true)); //ACID Transaction
            payload.Add("docs", new JArray(docs));

            var results = new List<CommitStatus>();

            var request = new Request("/:db/_bulk_docs", HttpVerbType.Post);
            request.AddUrlSegment("db", _name);
            request.AddContent(writer => writer.Write(payload.ToString()), HttpContentType.Json);


            _connection.Execute(request, response =>
            {
                var content = response.Content.ReadToEnd();
                var resultDocs = JArray.Parse(content);

                results.AddRange(resultDocs.Select(x =>
                {
                    var status = new CommitStatus()
                    {
                        Id = (string)x["id"],
                        Rev = (string)x["rev"],
                        Ok = (bool)x["ok"]
                    };

                    if (!status.Ok)
                    {
                        throw new Exception("failed to bulk update");
                    }

                    return status;
                }));
            });
            //TODO: partial update the object before exit of method via serializer
            return results;
        }
    }
}