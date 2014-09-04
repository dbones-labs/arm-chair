

namespace ArmChair
{
    using Tasks;
    using System.Collections.Generic;

    public static class Extenstions
    {
        public static string Format(this string str, params object[] parameters)
        {
            return string.Format(str, parameters);
        }


        /// <summary>
        /// force the iteration over a list (use this to force the <see cref="PipilineExecutor{T}.Execute"/> to run)
        /// </summary>
        /// <typeparam name="T">the type in the list</typeparam>
        /// <param name="collection">the list to iterate over</param>
        public static void Force<T>(this IEnumerable<T> collection)
        {
            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext()) { }
        }

        /// <summary>
        /// creates a <see cref="PipilineExecutor{T}"/> with tasks to run
        /// </summary>
        /// <typeparam name="T">the type to work with</typeparam>
        /// <param name="tasks">tasks to add to the pipeline</param>
        /// <returns>a configured pipeline</returns>
        public static PipilineExecutor<T> CreatePipeline<T>(this IEnumerable<IPipeTask<T>> tasks)
        {
            var pipeline = new PipilineExecutor<T>();
            foreach (var registration in tasks)
            {
                pipeline.Register(registration);
            }
            return pipeline;
        }

    }
}
