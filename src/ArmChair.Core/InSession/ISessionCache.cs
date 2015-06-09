// Copyright 2013 - 2015 dbones.co.uk (David Rundle)
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