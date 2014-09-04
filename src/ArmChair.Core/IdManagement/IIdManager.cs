using System;

namespace ArmChair.IdManagement
{
    public interface IIdManager
    {
        Key GenerateId(Type type);
        Key GetId(object instance);
        void SetId(object instance, object value);
        Key GetKeyFromId(Type type, object id);
    }
}