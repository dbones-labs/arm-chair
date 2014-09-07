using System;

namespace ArmChair.IdManagement
{
    using Utils;

    public class ShortStringIdManager : IIdManager
    {
        private readonly IIdentityGenerator _identityGenerator;

        public ShortStringIdManager()
        {
            _identityGenerator = new ShortGuidGenerator();
        }

        public Key GenerateId(Type type)
        {
            return new ShortGuidKey((string)_identityGenerator.GenerateId());
        }

        public Key GetFromId(Type type, object id)
        {
            return id == null ? null : new ShortGuidKey((string)id);
        }

        public Key GetFromCouchDbId(Type type, string id)
        {
            return new ShortGuidKey(id);
        }
    }
}