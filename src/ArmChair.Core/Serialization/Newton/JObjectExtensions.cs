namespace ArmChair.Serialization.Newton
{
    using Newtonsoft.Json.Linq;

    public static class JObjectExtensions
    {
        public static void RenameProperty(this JObject jObject, string oldName, string newName)
        {
            var property = jObject.Property(oldName);
            var value = property.Value;
            property.Remove();

            jObject.Add(newName, value);
        }
    }
}