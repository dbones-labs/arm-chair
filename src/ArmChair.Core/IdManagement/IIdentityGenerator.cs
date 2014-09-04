namespace ArmChair.IdManagement
{
    public interface IIdentityGenerator
    {
        object GenerateId();
        bool IsValidId(object value);
    }
}