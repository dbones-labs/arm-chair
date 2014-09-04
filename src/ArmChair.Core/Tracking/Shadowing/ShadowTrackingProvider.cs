namespace ArmChair.Tracking.Shadowing
{
    using System.Collections.Generic;
    using Utils.Comparing;
    using Utils.Copying;

    public class ShadowTrackingProvider : ITrackingProvider
    {
        readonly IDictionary<object, TrackingEntry> _trackedEntries = new Dictionary<object, TrackingEntry>();
        readonly ShadowCopier _copier = new ShadowCopier();
        readonly Comparer _comparer = new Comparer();

        public IEnumerable<TrackingEntry> TrackedEntries { get { return _trackedEntries.Values; } }

        public object TrackInstance(object instance)
        {
            var entry = new TrackingEntry()
            {
                ShadowCopy = _copier.Copy(instance),
                Instance = instance
            };

            _trackedEntries.Add(instance, entry);

            return instance;
        }

        public void Reset(object instance)
        {
            TrackingEntry entry;
            if (_trackedEntries.TryGetValue(instance, out entry))
            {
                entry.ShadowCopy = _copier.Copy(instance);
            }
        }

        public bool HasChanges(object instance)
        {
            TrackingEntry entry;
            return _trackedEntries.TryGetValue(instance, out entry)
                && _comparer.AreEqual(instance, entry.ShadowCopy);
        }

        public void CeaseTracking(object instance)
        {
            if (_trackedEntries.ContainsKey(instance))
            {
                _trackedEntries.Remove(instance);
            }
        }
    }
}
