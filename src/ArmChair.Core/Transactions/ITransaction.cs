namespace ArmChair.Transactions
{
    using System;

    public interface ITransaction : IDisposable
    {
        void CompleteCommit();
        void Rollback(Exception causeException);
    }
}