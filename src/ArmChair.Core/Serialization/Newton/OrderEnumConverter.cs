namespace ArmChair.Serialization.Newton
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// used to ensure we get the text for the order by enum
    /// </summary>
    public class OrderEnumConverter : StringEnumConverter
    {
        private readonly Type orderEnumType = typeof(Order);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var strvalue = ((Order) value) == Order.Asc ? "asc" : "desc";
                writer.WriteValue(strvalue);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == orderEnumType;
        }
    }
}