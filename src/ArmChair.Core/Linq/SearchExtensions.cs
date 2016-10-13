namespace ArmChair.Linq
{
    using System.Linq;
    using IQToolkit;

    public static class SearchExtensions
    {
        public static IQueryable<T> Search<T>(this ISession session) where T : class
        {
            return new Query<T>(new QueryProvider<T>((Session)session));
        }
    }
}