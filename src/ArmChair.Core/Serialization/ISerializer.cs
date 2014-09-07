namespace ArmChair.Serialization
{
    using System;

    public interface ISerializer
    {
        object Deserialize(string json);
        object Deserialize(string json, Type type);
        T Deserialize<T>(string json);
        string Serialize(object instance);
        string Serialize(object instance, Type type);
        string Serialize<T>(T instance);
    }
}