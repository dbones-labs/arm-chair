using System;

namespace ArmChair.IdManagement
{
    public class SimpleIdManager : IIdManager
    {
        private readonly IIdAccessor _idAccessor;
        private readonly IIdentityGenerator _identityGenerator;

        public SimpleIdManager(IIdAccessor idAccessor)
        {
            _idAccessor = idAccessor;
            _identityGenerator = new GuidGenerator();
        }

        public Key GenerateId(Type type)
        {
            return new GlobalKey((string)_identityGenerator.GenerateId());
        }

        public Key GetId(object instance)
        {
            var id = _idAccessor.GetId(instance);
            if (!_identityGenerator.IsValidId(id))
            {
                return null;
            }
            var key = new GlobalKey(id);
            return key;
        }

        public void SetId(object instance, object value)
        {
            _idAccessor.SetId(instance, value);
        }

        public Key GetKeyFromId(Type type, object id)
        {
            var key = new GlobalKey(id);
            return key;
        }
    }
}