namespace ArmChair.Http
{
    using System.Net;

    /// <summary>
    /// RFC 2109
    /// </summary>
    public class CookieAuthentication : IAuthentication
    {
        private Cookie _authSession;


        public CookieAuthentication(
            string serverUrl,
            string userName,
            string password)
        {

            var connection = new Connection(serverUrl);
            var request = new FormRequest("/_session", HttpVerbType.Post);
            request.AddFormParameter("name", userName);
            request.AddFormParameter("password", password);

            connection.Execute(request, response =>
            {
                if (response.Status == HttpStatusCode.OK)
                {
                    _authSession = response.Cookies["AuthSession"];
                }

            });
        }

        public void Apply(IRequest request)
        {
            request.AddCookie(_authSession);
        }
    }
}