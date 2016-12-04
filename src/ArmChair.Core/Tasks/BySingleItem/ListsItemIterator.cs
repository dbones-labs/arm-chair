namespace ArmChair.Tasks.BySingleItem
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// instead of the default <see cref="YeildingItemIterator{T}"/> which yields the result, this creates
    /// a new list which store the results.
    /// </summary>
    public class ListsItemIterator<T> : IItemIterator<T>
    {
        public IEnumerable<T> Execute(IEnumerable<T> items, Func<T, Action, T> execute)
        {
            var results = new List<T>();
            foreach (var item in items)
            {
                var skipped = false;
                Action skip = () => skipped = true;
                var result = execute(item, skip);
                if (!skipped)
                {
                    results.Add(result);
                }
            }
            return results;
        }
    }
}