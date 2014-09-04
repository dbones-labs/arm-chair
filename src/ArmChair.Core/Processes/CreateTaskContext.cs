namespace ArmChair.Processes
{
    using EntityManagement;
    using IdManagement;
    using InSession;

    public class CreateTaskContext
    {
        public CreateTaskContext(Database database,
            IIdManager idManager,
            IRevisionAccessor revisionAccessor,
            ISessionCache sessionCache)
        {
            SessionCache = sessionCache;
            RevisionAccessor = revisionAccessor;
            IdManager = idManager;
            Database = database;
        }

        public Database Database { get; private set; }
        public IIdManager IdManager { get; private set; }
        public IRevisionAccessor RevisionAccessor { get; private set; }
        public ISessionCache SessionCache { get; private set; }
    }
}