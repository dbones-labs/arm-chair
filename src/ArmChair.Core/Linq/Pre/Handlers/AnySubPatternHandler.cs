namespace ArmChair.Linq.Pre.Handlers
{
    using System;
    using System.Linq;

    public class AnySubPatternHandler : SubPatternHandlerBase
    {
        public AnySubPatternHandler(): base(objects => objects.Any()) { }
        
        public override void Update(ProcessingLinqContext ctx)
        {
            throw new NotImplementedException();
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return true;
        }
    }
}