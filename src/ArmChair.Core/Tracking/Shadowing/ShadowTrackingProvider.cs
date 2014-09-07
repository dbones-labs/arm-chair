// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
