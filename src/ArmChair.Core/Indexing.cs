namespace ArmChair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;

    public class Indexing
    {
        private readonly CouchDb _couchDb;

        public Indexing(CouchDb couchDb)
        {
            _couchDb = couchDb;
        }

        /// <summary>
        /// creates an index on the database, which can be used with Mongo/Linq queries.
        /// </summary>
        /// <param name="index">the index</param>
        /// <exception cref="ArgumentNullException">index</exception>
        public virtual void Create(IndexEntry index)
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

            //todo: check the response
            var response = _couchDb.CreateIndex(request);
        }
    }

    public static class IndexingExtensions
    {
        public static void Create<T>(this Indexing indexing, Action<IndexEntry<T>> config)
        {
            var index = new IndexEntry<T>();
            config(index);
            index.Compile();
            indexing.Create(index);
        }
    }
}