namespace ArmChair.Utils.Hashing
{
    /// <summary>
    /// this will take a string and tolower it
    /// </summary>
    public class ToLowerPassThroughHash : IHash
    {
        public string ComputeHash(string content)
        {
            return content.ToLower();
        }
    }
}