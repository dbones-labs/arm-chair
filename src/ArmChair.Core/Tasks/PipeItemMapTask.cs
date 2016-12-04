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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// this represents a task. these are applied normally within a Pipeline
    /// </summary>
    /// <typeparam name="T">what type the task will be executed against</typeparam>
    [Obsolete("this will be removed in the future, plesae use TaskOnItem instead")]
    public abstract class PipeItemMapTask<T> : IPipeTask<T>
    {
        /// <summary>
        /// a function to see if the task will support the current instance,
        /// this is normally called before the execute
        /// </summary>
        public virtual bool CanHandle(T item)
        {
            return true;
        }

        /// <summary>
        /// executes the tasks logic
        /// </summary>
        /// <param name="item">the item on which the task will operate on</param>
        public abstract IEnumerable<T> Execute(T item);

        public IEnumerable<T> Execute(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!CanHandle(item))
                {
                    yield return item;
                    continue;
                }

                var results = Execute(item);
                foreach (var result in results.Where(x => x != null))
                {
                    yield return result;
                }
            }
        }
    }
}