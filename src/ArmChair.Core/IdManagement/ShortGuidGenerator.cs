using ArmChair.Utils;

namespace ArmChair.IdManagement
{
    public class ShortGuidGenerator : IIdentityGenerator
    {
        public object GenerateId()
        {
            return ShortGuid.NewGuid().ToString();
        }

        public bool IsValidId(object value)
        {
            return value is string;
        }
    }
}