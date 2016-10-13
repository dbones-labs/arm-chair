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
namespace ArmChair.Tasks
{
    using System.Collections.Generic;

    /// <summary>
    /// this represents a task. these are applied normally within a Pipeline
    /// </summary>
    /// <typeparam name="T">what type the task will be executed against</typeparam>
    public interface IPipeTask<T> : IPipeTask<IEnumerable<T>, IEnumerable<T>>
    {
    }

    /// <summary>
    /// this represents a task. these are applied normally within a Pipeline
    /// </summary>
    public interface IPipeTask<in TIn, out TOut>
    {
        /// <summary>
        /// executes the tasks logic
        /// </summary>
        /// <param name="input">the input on which the task will operate on</param>
        TOut Execute(TIn input);
    }
}