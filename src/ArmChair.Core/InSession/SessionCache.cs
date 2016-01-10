// Copyright 2014 - dbones.co.uk (David Rundle)
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
using System.Collections.Generic;
using ArmChair.IdManagement;

namespace ArmChair.InSession
{
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
        
        public void Clear()
        {
            _objectsInSession.Clear();
        }
    }
}
