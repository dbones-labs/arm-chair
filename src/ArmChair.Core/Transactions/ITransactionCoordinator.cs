namespace ArmChair.Transactions
{
    using System.Collections.Generic;

    public interface ITransactionCoordinator
    {
        ITransaction Setup(IEnumerable<object> items);
    }
}