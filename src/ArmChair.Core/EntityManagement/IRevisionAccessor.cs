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
namespace ArmChair.EntityManagement
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utils;

    /// <summary>
    /// Store information about the Revision field within each 
    /// instance to be stored in the database
    /// </summary>
    public interface IRevisionAccessor
    {
        /// <summary>
        /// Autoscan for the Revision field (else the Revision needs to be setup explicitly)
        /// </summary>
        void AllowAutoScanningForRevision();

        void DisableAutoScanningForRevision();

        /// <summary>
        /// The scanning patten to be used to autoscan for the Revision field
        /// </summary>
        void SetUpRevisionPattern(Func<Type, string> pattern);

        /// <summary>
        /// Set the Revision directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Revision against</typeparam>
        /// <param name="field">Revision Field</param>
        void SetUpRevision<T>(FieldInfo field);

        /// <summary>
        /// Set the Revision directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Revision against</typeparam>
        /// <param name="fieldName">name of the Revision field</param>
        void SetUpRevision<T>(string fieldName);

        void SetUpRevision(Type type, FieldInfo field);

        /// <summary>
        /// Set the Revision directly against a Type
        /// </summary>
        /// <typeparam name="T">the tpye which to set the Revision against</typeparam>
        /// <param name="property">The Revision property, this will then use the Auto back field</param>
        void SetUpRevision<T>(Expression<Func<T, object>> property);

        /// <summary>
        /// Gets the Revision for an instance
        /// </summary>
        /// <param name="instance">the object instance to get the Revision of</param>
        /// <returns>the Revision value</returns>
        object GetRevision(object instance);

        /// <summary>
        /// Set the Revision of an instance
        /// </summary>
        /// <param name="instance">the instance to set its Id</param>
        /// <param name="id">the value to set it too</param>
        void SetRevision(object instance, object id);

        /// <summary>
        /// Get the Revision field
        /// </summary>
        /// <param name="type">the type to find its Revision field</param>
        /// <returns>the Revision field</returns>
        FieldMeta GetRevisionField(Type type);
    }
}