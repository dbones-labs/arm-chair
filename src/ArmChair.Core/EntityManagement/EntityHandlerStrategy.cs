using System;
using ArmChair.IdManagement;

namespace ArmChair.EntityManagement
{
    public class EntityHandlerStrategy : IEntityHandlerStrategy
    {
        private readonly IIdAccessor _idAccessor;

        public EntityHandlerStrategy(IdAccessor idAccessor)
        {
            _idAccessor = idAccessor;
        }

        public bool IsEntityRoot(object instance)
        {
            if (instance == null)
            {
                return false;
            }
            Type type = instance.GetType();
            
            //all types require an id
            return _idAccessor.GetIdField(type) != null;
        }

    }
}