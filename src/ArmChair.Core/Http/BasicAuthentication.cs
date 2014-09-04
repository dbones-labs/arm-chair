namespace ArmChair.Http
{
    using System;
    using System.Text;


    /// <summary>
    /// RFC 2617
    /// </summary>
    public class BasicAuthentication : IAuthentication
    {
        private string authHeader;
        public BasicAuthentication(string userName, string password)
        {
            var encoded = Convert.ToBase64String(Encoding.Default.GetBytes(userName + ":" + password));
            authHeader = "Basic " + encoded;
        }

        public void Apply(IRequest request)
        {
            request.AddHeader("Authorization", authHeader);
        }
    }
}
