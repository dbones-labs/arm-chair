namespace ArmChair.Serialization
{
    using System;

    public interface ISerializer
    {
        object Deserialize(string json);
        object Deserialize(string json, Type type);
        string Serialize(object instance);
        string Serialize(object instance, Type type);
    }
}