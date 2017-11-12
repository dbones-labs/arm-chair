namespace Todo.Service.Infrastructure.Behaviours
{
    using ArmChair;
    using MediatR;
    using System.Threading.Tasks;

    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ISession _session;

        public UnitOfWorkBehavior(ISession session)
        {
            _session = session;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            _session.Commit();

            return response;
        }
    }
}
