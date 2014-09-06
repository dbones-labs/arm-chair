using System;

namespace ArmChair.IdManagement
{
    public interface IIdManager
    {
        Key GenerateId(Type type);
        Key GetFromId(Type type, object id);
        Key GetFromCouchDbId(Type type, string id);
    }
}