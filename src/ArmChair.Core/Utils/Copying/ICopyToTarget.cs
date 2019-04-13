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
namespace ArmChair.Utils.Copying
{
    /// <summary>
    /// Copy values from the source to the target.
    /// </summary>
    public interface ICopyToTarget
    {
        /// <summary>
        /// copy across all the vales from source to destination.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        void Copy(object source, object destination);


        void Configure(ShadowCopier copier);
    }
}