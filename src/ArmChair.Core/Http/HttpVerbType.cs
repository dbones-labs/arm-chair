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
namespace ArmChair.Http
{
    /// <summary>
    /// HTTP Verbs which can be used with a request
    /// </summary>
    public enum HttpVerbType
    {
        /// <summary>
        /// Delete a resource
        /// </summary>
        Delete,
        /// <summary>
        /// Post is mostly assoicated with a new resouce
        /// </summary>
        Post,
        /// <summary>
        /// Put is mostly associated with a update to a resouce
        /// </summary>
        Put,
        /// <summary>
        /// Get a resouce
        /// </summary>
        Get
    }
}