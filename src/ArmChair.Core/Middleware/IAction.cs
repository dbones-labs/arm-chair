namespace ArmChair.Middleware
{
    using System.Threading.Tasks;

    public interface IAction<T>
    {
        Task Execute(T context, Next<T> next);
    }
    
    public interface IAction<TIn, TOut>
    {
        Task<TOut> Execute(TIn context, Next<TIn, TOut> next);
    }
}