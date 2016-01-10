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
namespace ArmChair.IdManagement
{
    using System;

    /// <summary>
    /// Handles the Id strategies to be used for each type. NOTE this uses
    /// a <see cref="Key"/> to manage Id's in this framework.
    /// </summary>
    public interface IIdManager
    {
        /// <summary>
        /// New Key/Id for a type
        /// </summary>
        /// <param name="type">Type to generate an Id for</param>
        /// <returns>ID Key</returns>
        Key GenerateId(Type type);

        /// <summary>
        /// Given the Type and ID value, this will return the valid Key
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="id">value of the Id</param>
        /// <returns>Key to use</returns>
        Key GetFromId(Type type, object id);

        /// <summary>
        /// Gets the key from the CoucbDb string value
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="id">couchdb value</param>
        /// <returns>Key to use</returns>
        Key GetFromCouchDbId(Type type, string id);
    }
}