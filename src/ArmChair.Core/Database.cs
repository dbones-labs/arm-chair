namespace ArmChair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Http;
    using Serialization;

    public class Database
    {
        private readonly string _name;
        private readonly IConnection _connection;
        private readonly ISerializer _serializer;

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
            //TODO: partial update the object before exit of method via serializer
            return results;
        }
    }
}