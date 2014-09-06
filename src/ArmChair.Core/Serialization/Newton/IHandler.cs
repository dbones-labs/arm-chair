namespace ArmChair.Serialization.Newton
{
    using System;

    public interface IHandler
    {
        Type HandlesType { get; }
        void Handle(SerializerContext context, Serializer serializer);
    }
}