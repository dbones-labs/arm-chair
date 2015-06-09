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
namespace ArmChair.Tracking
{
    /// <summary>
    /// tracks instances to see if they have been modified
    /// </summary>
    public interface ITrackingProvider
    {
        /// <summary>
        /// add instance to be tracked
        /// </summary>
        /// <param name="instance">instance to be tracked</param>
        /// <returns>returns the tracked instance, use this reference to the instance</returns>
        object TrackInstance(object instance);

        /// <summary>
        /// reset an instance, it will take these values as being the baseline
        /// </summary>
        /// <param name="instance">the instance to use as a baseline</param>
        void Reset(object instance);
        
        /// <summary>
        /// see if there are any chanes to a tracked instance
        /// </summary>
        /// <param name="instance">the current instance, to check against</param>
        /// <returns>true if there have been any changes since it was registed for tracking</returns>
        bool HasChanges(object instance);
        
        /// <summary>
        /// stop tracking an instance, do this to free up memory
        /// </summary>
        /// <param name="instance">instance which we do not want to track anymore</param>
        void CeaseTracking(object instance);
    }
}
