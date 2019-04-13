namespace ArmChair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Commands;
    using Utils.Hashing;

    public class Indexing
    {
        private readonly CouchDb _couchDb;
        private readonly IHash _hash;

        public Indexing(CouchDb couchDb, IHash hash)
        {
            _couchDb = couchDb;
            _hash = hash;
        }


        /// <summary>
        /// creates an index on the database, which can be used with Mongo/Linq queries.
        /// </summary>
        /// <param name="index">the index</param>
        /// <exception cref="ArgumentNullException">index</exception>
        public virtual void Create(IndexEntry index)
        {
            if (index == null) throw new ArgumentNullException(nameof(index));

            Compile(index);

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

        /// <summary>
        /// sets properties with generated values (only if they have not )
        /// </summary>
        protected void Compile(IndexEntry indexEntry)
        {
            if (string.IsNullOrEmpty(indexEntry.DesignDocument) && indexEntry.Type != null)
            {
                indexEntry.DesignDocument = indexEntry.Type.GetTypeInfo().FullName;
            }
            
            if (string.IsNullOrEmpty(indexEntry.Name))
            {
                var entries = indexEntry.Index.Fields.Select(x =>
                {
                    var entry = x.First();
                    return $"{entry.Key}.{entry.Value}";
                });

                indexEntry.Name = _hash.ComputeHash(string.Join("-", entries));
            }
        }

    }

    public static class IndexingExtensions
    {
        public static void Create<T>(this Indexing indexing, Action<IndexEntry<T>> config)
        {
            var index = new IndexEntry<T>();
            config(index);
            indexing.Create(index);
        }
    }
}