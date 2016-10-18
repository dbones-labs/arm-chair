namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Serialization;

    internal class LocalCamelCaseNamingStrategy : CamelCaseNamingStrategy
    {
        private readonly Regex _camelcaseRegex = new Regex("(^|\\.|\\$)[A-Z]");

        public LocalCamelCaseNamingStrategy() : base (true, false)
        {
            
        }


        public override string GetDictionaryKey(string key)
        {
            return ResolvePropertyName(key);
        }

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

            var result = _camelcaseRegex.Replace(propertyName, m => m.ToString().ToLower());
            return result;
            //base.ResolvePropertyName(propertyName);
        }
    }
}