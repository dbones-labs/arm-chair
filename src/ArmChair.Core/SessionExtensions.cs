namespace ArmChair
{
    using System.Collections.Generic;

    public static class SessionExtensions
    {
        /// <summary>
        /// add a range of instances to the session
        /// </summary>
        /// <typeparam name="T">the instance type</typeparam>
        /// <param name="session">the session to use</param>
        /// <param name="instances">a colletion of objects</param>
        public static void AddRange<T>(this ISession session, IEnumerable<T> instances) where T: class
        {
            foreach (var instance in instances)
            {
                session.Add(instance);
            }
        }


        /// <summary>
        /// attach a range of instances to the session
        /// </summary>
        /// <typeparam name="T">the instance type</typeparam>
        /// <param name="session">the session to use</param>
        /// <param name="instances">a colletion of objects</param>
        public static void AttachRange<T>(this ISession session, IEnumerable<T> instances) where T : class
        {
            foreach (var instance in instances)
            {
                session.Attach(instance);
            }
        }

        /// <summary>
        /// remove a range of instances from the the database
        /// </summary>
        /// <typeparam name="T">the instance type</typeparam>
        /// <param name="session">the session to use</param>
        /// <param name="instances">a colletion of objects</param>
        public static void RemoveRange<T>(this ISession session, IEnumerable<T> instances) where T : class
        {
            foreach (var instance in instances)
            {
                session.Remove(instance);
            }
        }
    }
}