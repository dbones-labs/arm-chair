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
    using System.Linq;

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

        /// <summary>
        /// execute a mongo query against CouchDb, note this is under preview
        /// </summary>
        /// <typeparam name="T">the object type which is being queried</typeparam>
        /// <param name="query">the mongo query to execute</param>
        /// <returns>all items which satisfy the query</returns>
        IEnumerable<T> Query<T>(MongoQuery query) where T : class;

        /// <summary>
        /// basic support for linq over mongo queries, note this is under preview
        /// </summary>
        /// <typeparam name="T">the object type whichs to filter on</typeparam>
        /// <returns>all items which satisfy the query</returns>
        IQueryable<T> Query<T>(string index = null) where T : class;

    }


    public static class SessionExtensions
    {
        /// <summary>
        /// add a range of instances to the session
        /// </summary>
        /// <typeparam name="T">the instance type</typeparam>
        /// <param name="session">the session to use</param>
        /// <param name="instances">a colletion of objects</param>
        public static void AddRange<T>(this ISession session, IEnumerable<T> instances) where T: class
        {
            foreach (var instance in instances)
            {
                session.Add(instance);
            }
        }


        /// <summary>
        /// attach a range of instances to the session
        /// </summary>
        /// <typeparam name="T">the instance type</typeparam>
        /// <param name="session">the session to use</param>
        /// <param name="instances">a colletion of objects</param>
        public static void AttachRange<T>(this ISession session, IEnumerable<T> instances) where T : class
        {
            foreach (var instance in instances)
            {
                session.Attach(instance);
            }
        }

        /// <summary>
        /// remove a range of instances from the the database
        /// </summary>
        /// <typeparam name="T">the instance type</typeparam>
        /// <param name="session">the session to use</param>
        /// <param name="instances">a colletion of objects</param>
        public static void RemoveRange<T>(this ISession session, IEnumerable<T> instances) where T : class
        {
            foreach (var instance in instances)
            {
                session.Remove(instance);
            }
        }
    }

    public class MongoQuery
    {
        /// <summary>
        /// JSON object describing criteria used to select documents. More information provided in the section on selector syntax.
        /// </summary>
        public IDictionary<string, object> Selector { get; set; }

        /// <summary>
        /// Maximum number of results returned. Default is 25. Optional
        /// </summary>
        public long? Limit { get; set; }

        /// <summary>
        /// Skip the first ‘n’ results, where ‘n’ is the value specified. Optional
        /// </summary>
        public long? Skip { get; set; }


        public string Index { get; set; }

        public IList<IDictionary<string, Order>> Sort { get; set; }
    }


    public enum Order
    {
        Asc,
        Desc
    }

}