namespace ArmChair.Linq.Pre
{
    using System.Collections.Generic;
    using Handlers;

    public class ProcessingLinqContext
    {
        public ProcessingLinqContext(LinqQuery linqQuery)
        {
            LinqQuery = linqQuery;
            PreviousSubPatterns = new List<ISubPatternHandler>();
        }

        public IEnumerable<ISubPatternHandler> PreviousSubPatterns { get; private set; }
        public Method PreviousMethod { get; private set; }
        public Method CurrentMethod { get; private set; }
        public LinqQuery LinqQuery { get; private set; }
        //public Type BaseType { get; set; }

        public void SetCurrentMethod(Method currentMethod)
        {
            if (CurrentMethod != null)
            {
                PreviousMethod = CurrentMethod;
            }
            CurrentMethod = currentMethod;
        }


        public void HandledBy(IEnumerable<ISubPatternHandler> pattens)
        {
            PreviousSubPatterns = pattens;
        }
    }
}