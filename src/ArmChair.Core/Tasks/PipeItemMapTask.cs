namespace ArmChair.Tasks
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// this represents a task. these are applied normally within a Pipeline
    /// </summary>
    /// <typeparam name="T">what type the task will be executed against</typeparam>
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
                    yield break;
                }

                var results = Execute(item);
                foreach (var result in results.Where(x => !(x == null)))
                {
                    yield return result;
                }
            }
        }
    }
}