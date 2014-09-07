namespace ArmChair.Tasks
{
    using System.Collections.Generic;

    /// <summary>
    /// this represents a task. these are applied normally within a Pipeline
    /// </summary>
    /// <typeparam name="T">what type the task will be executed against</typeparam>
    public interface IPipeTask<T>
    {
        /// <summary>
        /// executes the tasks logic
        /// </summary>
        /// <param name="items">the items on which the task will operate on</param>
        IEnumerable<T> Execute(IEnumerable<T> items);
    }
}