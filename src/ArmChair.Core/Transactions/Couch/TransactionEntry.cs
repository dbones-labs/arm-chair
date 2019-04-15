namespace ArmChair.Transactions.Couch
{
    using System;

    public class TransactionEntry
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string ActualId { get; set; }
        public string ActualRev { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public string TransactionId { get; set; }
    }
}