namespace ArmChair.Middleware
{
    using System.Threading.Tasks;

    public delegate Task Next<in T>(T context);
    
    public delegate Task<TOut> Next<in TIn, TOut>(TIn context);
}