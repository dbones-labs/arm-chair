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
namespace ArmChair.EntityManagement
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utils;

    /// <summary>
    /// Store information about the Id field within each 
    /// instance to be stored in the database
    /// </summary>
    public interface IIdAccessor
    {
        /// <summary>
        /// Autoscan for the Id field (else the Id needs to be setup explicitly)
        /// </summary>
        void AllowAutoScanningForId();

        /// <summary>
        /// The scanning patten to be used to autoscan for the Id field
        /// </summary>
        void SetUpIdPattern(Func<Type, string> pattern);
        
        /// <summary>
        /// Set the Id directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Id against</typeparam>
        /// <param name="field">Id Field</param>
        void SetUpId<T>(FieldInfo field);

        /// <summary>
        /// Set the Id directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Id against</typeparam>
        /// <param name="fieldName">name of the Id field</param>
        void SetUpId<T>(string fieldName);

        /// <summary>
        /// Set the Id directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Id against</typeparam>
        /// <param name="property">The Id property, this will then use the Auto back field</param>
        void SetUpId<T>(Expression<Func<T, object>> property);
        
        /// <summary>
        /// Gets the Id for an instance
        /// </summary>
        /// <param name="instance">the object instance to get the Id of</param>
        /// <returns>the Id value</returns>
        object GetId(object instance);
        
        /// <summary>
        /// Set the Id of an instance
        /// </summary>
        /// <param name="instance">the instance to set its Id</param>
        /// <param name="id">the value to set it too</param>
        void SetId(object instance, object id);

        /// <summary>
        /// Get the Id field
        /// </summary>
        /// <param name="type">the type to find its Id field</param>
        /// <returns>the Id field</returns>
        FieldMeta GetIdField(Type type);
    }
}