namespace ArmChair.Serialization.Newton
{
    using System;
    using Newtonsoft.Json.Serialization;

    internal class LocalCamelCaseNamingStrategy : CamelCaseNamingStrategy
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return null;
            }

            if (propertyName.StartsWith("<", StringComparison.CurrentCultureIgnoreCase))
            {
                int index = propertyName.IndexOf(">", StringComparison.CurrentCultureIgnoreCase);
                propertyName = propertyName.Substring(1, index - 1);
            }

            if (propertyName.StartsWith("_", StringComparison.CurrentCultureIgnoreCase))
            {
                propertyName = propertyName.Substring(1, propertyName.Length - 1);
            }

            return base.ResolvePropertyName(propertyName);
        }
    }
}