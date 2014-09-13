namespace ArmChair.InSession
{
    using System.Collections.Generic;
    using IdManagement;

    /// <summary>
    /// Cache instances of entities which are being manipulated in this session.
    /// </summary>
    public interface ISessionCache
    {
        /// <summary>
        /// Attach the entry to this session
        /// </summary>
        /// <param name="entry"></param>
        void Attach(SessionEntry entry);
        
        /// <summary>
        /// Get the session Entry
        /// </summary>
        /// <param name="key">The unique Key</param>
        /// <returns>the entry if it exists, or null</returns>
        SessionEntry this[Key key] { get; }

        /// <summary>
        /// Renove entry via its unique key
        /// </summary>
        /// <param name="key">the unique key</param>
        void Remove(Key key);

        /// <summary>
        /// All the entries in this session
        /// </summary>
        IEnumerable<SessionEntry> Entries { get; }

        /// <summary>
        /// Clear down the session entries
        /// </summary>
        void Clear();
    }
}