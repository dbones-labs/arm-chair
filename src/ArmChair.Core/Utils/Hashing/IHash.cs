namespace ArmChair.Utils.Hashing
{
    /// <summary>
    /// craetes a hash
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// create a hash of a string
        /// </summary>
        /// <param name="content">the string to be hashed</param>
        /// <returns>the hashed string in a string</returns>
        string ComputeHash(string content);
    }
}
