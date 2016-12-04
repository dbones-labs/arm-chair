namespace ArmChair.Tasks.BySingleItem
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// a way to process the items 1 by 1.
    /// orverride the Execute method you want.
    /// </summary>
    /// <typeparam name="T">the item type which will be executed against</typeparam>
    public abstract class TaskOnItem<T> : IPipeTask<T>
    {
        private IItemIterator<T> _iterator;

        /// <summary>
        /// creates an instace of the task
        /// </summary>
        /// <param name="iterator">supply the iterator, the defult will be uesed if not supplied</param>
        protected TaskOnItem(IItemIterator<T> iterator = null)
        {
            if (iterator == null)
            {
                iterator = new YeildingItemIterator<T>();
            }
            _iterator = iterator;
        }

        public IEnumerable<T> Execute(IEnumerable<T> items)
        {
            return _iterator.Execute(items, Execute);
        }

        public virtual T Execute(T item, Action skip)
        {
            return Execute(item);
        }

        public virtual T Execute(T item)
        {
            return item;
        }
    }
}