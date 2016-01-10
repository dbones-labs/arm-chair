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
namespace ArmChair
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// An active session with the database, using a unit of work, any attach object will be
    /// tracked for changes, and these changes will be commited to the database.
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// Add a new object to the database, NOTE an ID will be assigned 
        /// if it has not already been assigned.
        /// </summary>
        /// <param name="instance">The instace to add</param>
        void Add<T>(T instance) where T : class;

        /// <summary>
        /// Remove the instance from the database
        /// </summary>
        /// <param name="instance">The instance to remove</param>
        void Remove<T>(T instance) where T : class;

        /// <summary>
        /// Attach an existing object to this session, allowing you to make updates to it
        /// during this session
        /// </summary>
        /// <param name="instance">The instance to attach to this session</param>
        void Attach<T>(T instance) where T : class;
        
        /// <summary>
        /// Load an number of entities via its ID, (reconmended to use with an indexing service)
        /// </summary>
        /// <typeparam name="T">the type of the object</typeparam>
        /// <param name="ids">list of all the IDs for the entities to load</param>
        /// <returns>a collection containing all the loaded entities</returns>
        IEnumerable<T> GetByIds<T>(IEnumerable ids) where T : class;

        /// <summary>
        /// Load a single entity via ids ID.
        /// </summary>
        /// <typeparam name="T">The entity Type</typeparam>
        /// <param name="id">the ID of the entity to load from the database</param>
        /// <returns>the loaded entity</returns>
        T GetById<T>(object id) where T : class;

        /// <summary>
        /// Commit all changes to the database.
        /// </summary>
        void Commit();
    }
}