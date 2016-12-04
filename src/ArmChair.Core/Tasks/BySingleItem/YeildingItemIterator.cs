namespace ArmChair.Tasks.BySingleItem
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// iterates over all the items in the list using yeild to output the result.
    /// </summary>
    public class YeildingItemIterator<T> : IItemIterator<T>
    {
        public IEnumerable<T> Execute(IEnumerable<T> items, Func<T, Action, T> execute)
        {
            foreach (var item in items)
            {
                var skipped = false;
                Action skip = () => skipped = true;
                var result = execute(item, skip);
                if (!skipped)
                {
                    yield return result;
                }
            }
        }
    }
}