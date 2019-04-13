namespace ArmChair.Utils.Hashing
{
    using System;
    using System.Security.Cryptography;
    using System.Text;


    /// <summary>
    /// simple SHA1 hashing algorithm.
    /// </summary>
    public class Sha1Hash : IHash
    {
        public string ComputeHash(string content)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(content);
                data = sha1.ComputeHash(data);
                return BitConverter.ToString(data).Replace("-", "");
            }
        }
    }
}