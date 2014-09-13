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
namespace ArmChair.IdManagement
{
    /// <summary>
    /// Generate ID's to be used
    /// </summary>
    public interface IIdentityGenerator
    {
        /// <summary>
        /// Generate a new ID Value
        /// </summary>
        /// <returns>ID value</returns>
        object GenerateId();

        /// <summary>
        /// confirm if a value is a valid ID
        /// </summary>
        /// <param name="value">value to check</param>
        /// <returns>true if it is OK</returns>
        bool IsValidId(object value);
    }
}