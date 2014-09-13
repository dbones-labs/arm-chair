namespace ArmChair
{
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
        public Settings Settings { get; private set; }

        /// <summary>
        /// create a session with the database, which will allow 
        /// you to interact with it in a Unit Of Work style
        /// </summary>
        /// <returns>active session</returns>
        public ISession CreateSession()
        {
            //session level
            var tracker = new ShadowTrackingProvider();
            var sessionCache = new SessionCache();

            var session = new Session(Settings.LoadPipeline, Settings.CommitPipeline, Settings.IdManager, Settings.IdAccessor, tracker, sessionCache);
            return session;
        }
    }
}