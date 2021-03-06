﻿// Copyright 2014 - dbones.co.uk (David Rundle)
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
namespace ArmChair.IdManagement
{
    /// <summary>
    /// allows the unique handling of Ids.
    /// it stores the Id value and the CouchDb value
    /// </summary>
    public abstract class Key
    {
        /// <summary>
        /// the Id value
        /// </summary>
        public abstract object Id { get; }

        /// <summary>
        /// couchdb value of the id
        /// </summary>
        public abstract string CouchDbId { get; }

        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            var otherKey = obj as Key;
            if (otherKey == null)
            {
                return false;
            }
            return otherKey.GetHashCode() == GetHashCode();
        }

        /// <summary>
        /// The string value is used to save to the CouchDb.
        /// </summary>
        /// <returns>it should return a fully unique value.</returns>
        public abstract override string ToString();

    }
}