namespace ArmChair
{
    using Commands;
    using EntityManagement;
    using Http;
    using IdManagement;
    using Processes.Commit;
    using Processes.Load;
    using Serialization;
    using Serialization.Newton;

    public class Settings
    {
        public Settings(string databaseName, Connection connection)
        {
            //global
            IdAccessor = new IdAccessor();
            IdManager = new ShortStringIdManager();
            RevisionAccessor = new RevisionAccessor();
            Serializer = new Serializer(IdAccessor, RevisionAccessor);

            CouchDb = new CouchDb(databaseName, connection, Serializer);

            LoadPipeline = new LoadPipeline(CouchDb, IdManager, IdAccessor, RevisionAccessor);
            CommitPipeline = new CommitPipeline(CouchDb, IdManager, RevisionAccessor);
            
        }


        public IIdAccessor IdAccessor { get; private set; }
        public IIdManager IdManager { get; private set; }
        public IRevisionAccessor RevisionAccessor { get; private set; }
        public ISerializer Serializer { get; private set; }

        public LoadPipeline LoadPipeline { get; private set; }
        public CommitPipeline CommitPipeline { get; private set; }
        public CouchDb CouchDb { get; private set; }


    }
}