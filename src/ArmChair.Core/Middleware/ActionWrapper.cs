namespace ArmChair.Middleware
{
    using System;
    using System.Threading.Tasks;

    public class ActionWrapper<T> : IAction<T>
    {
        private readonly Func<T, Next<T>, Task> _action;

        public ActionWrapper(Func<T, Next<T>, Task> action)
        {
            _action = action;
        }

        public async Task Execute(T context, Next<T> next)
        {
            await _action(context, next);
        }
    }
}