using System.Collections.Generic;
using ArmChair.IdManagement;

namespace ArmChair.InSession
{


    public interface ISessionCache
    {
        void Attach(SessionEntry entry);
        SessionEntry this[Key key] { get; }
        void Remove(Key key);
        IEnumerable<SessionEntry> Entries { get; }
    }

    public class SessionCache : ISessionCache
    {
        private readonly IDictionary<Key, SessionEntry> _objectsInSession = new Dictionary<Key, SessionEntry>();

        public void Attach(SessionEntry entry)
        {
            var existing = this[entry.Key];
            if (existing == null)
            {
                _objectsInSession.Add(entry.Key, entry);
                return;
            }
            
            //TODO: consider the correct behaviour
            //consider logic about adding, if the item exists
            //updating if the item is deleted.

            existing.Action = entry.Action;
        }

        public SessionEntry this[Key key]
        {
            get
            {
                SessionEntry entry;
                _objectsInSession.TryGetValue(key, out entry);
                return entry;
            }
        }

        public void Remove(Key key)
        {
            _objectsInSession.Remove(key);
        }

        public IEnumerable<SessionEntry> Entries { get { return _objectsInSession.Values; } }
    }
}
