namespace ArmChair
{
    public interface IDocumentify
    {
        T AggregateRoot<T>(T instance = default(T)) where T : class;
    }
}