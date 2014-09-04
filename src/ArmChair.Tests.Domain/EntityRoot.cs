namespace ArmChair.Tests.Domain
{
    /// <summary>
    /// base for all domain object which use this Uow
    /// </summary>
    public abstract class EntityRoot
    {
        public virtual string Id { get; set; }
        public virtual string Rev { get; set; }
    }
}
