namespace ArmChair.Linq
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        public static bool ComparedTo(this string str, string compairedWith)
        {
            return string.Compare(str, compairedWith, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}