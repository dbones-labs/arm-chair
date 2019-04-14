namespace ArmChair.Middleware
{
    using System;

    internal class PipeItem
    {
        public PipeItem(object givenInstance)
        {
            GivenInstance = givenInstance;
            PipeItemType = PipeItemType.Instance;
        }

        public object GivenInstance { get; }

        public PipeItemType PipeItemType { get; }
    }
}