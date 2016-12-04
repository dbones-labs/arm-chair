namespace ArmChair.Tasks.BySingleItem
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// this is used to iterate over the items calling the execute func on each item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemIterator<T>
    {
        /// <summary>
        /// iterate over all the items, calling execute on each item.
        /// </summary>
        /// <param name="items">the collection of items</param>
        /// <param name="execute">
        /// an execute method, p1 => the items, p2 => a callback to remove the current item from 
        /// the list, return is the new item
        /// </param>
        /// <returns>the result of executing the function against all the items in the collection</returns>
        IEnumerable<T> Execute(IEnumerable<T> items, Func<T, Action, T> execute);
    }
}