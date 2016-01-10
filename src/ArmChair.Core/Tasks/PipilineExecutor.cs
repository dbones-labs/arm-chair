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
    using System.Linq;

    /// <summary>
    /// creates a pipeline with yielding. This class allows to multiple tasks to
    /// be executed against each item.
    /// </summary>
    /// <typeparam name="T">The type of item which will be yielded over</typeparam>
    public class PipilineExecutor<T>
    {
        readonly List<IPipeTask<T>> _tasks = new List<IPipeTask<T>>();

        /// <summary>
        /// provide a task to be applied in this pipeline
        /// </summary>
        /// <typeparam name="TTask">the task type</typeparam>
        /// <param name="task">the task to apply on each item</param>
        public virtual void Register<TTask>(TTask task) where TTask : IPipeTask<T>
        {
            _tasks.Add(task);
        }

        public IEnumerable<T> Execute(IEnumerable<T> items)
        {
            var mapped = items;

            foreach (var taskRunner in _tasks)
            {
                mapped = taskRunner.Execute(mapped);
            }

            return mapped.ToList(); //execute.
        }
    }
}
