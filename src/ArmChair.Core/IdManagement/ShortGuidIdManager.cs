using System;

namespace ArmChair.IdManagement
{
    using Utils;

    public class ShortGuidIdManager : IIdManager
    {
        private readonly IIdentityGenerator _identityGenerator;

        public ShortGuidIdManager()
        {
            _identityGenerator = new ShortGuidGenerator();
        }

        public Key GenerateId(Type type)
        {
            return new ShortGuidKey((string)_identityGenerator.GenerateId());
        }

        public Key GetFromId(Type type, object id)
        {
            return new ShortGuidKey((ShortGuid)id);
        }

        public Key GetFromCouchDbId(Type type, string id)
        {
            return new ShortGuidKey(id);
        }
    }
}