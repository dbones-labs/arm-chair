namespace ArmChair.Linq.Pre.Handlers
{
    using System;
    using System.Linq;

    public class AllSubPatternHandler : SubPatternHandlerBase
    {
        public AllSubPatternHandler() : base(objects => objects.All(p => true)) { }
        
        public override void Update(ProcessingLinqContext ctx)
        {
            throw new NotImplementedException();
        }

        public override bool IndexQueryCompleted(ProcessingLinqContext ctx)
        {
            return false;
        }
    }
}