namespace ArmChair.Linq.Pre
{
    using System.Collections.Generic;
    using Handlers;

    /// <summary>
    /// stores a lookup to all the pattern handlers
    /// </summary>
    public static class SubPatternRegistry
    {
        public static List<ISubPatternHandler> Handlers  { get; set; } 

        static SubPatternRegistry()
        {
            Handlers = new List<ISubPatternHandler>
            {
                new AllSubPatternHandler(),
                new AnySubPatternHandler(),
                new FirstSubPatternHandler(),
                new OrderBySubPatternHandler(),
                new ThenBySubPatternHandler(),
                new SkipSubPatternHandler(),
                new SingleSubPatternHandler(),
                new TakeSubPatternHandler(),
                new WhereSubPatternHandler()
            };
        }
    }
}