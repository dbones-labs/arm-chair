namespace ArmChair.Http
{
    using System.Net.Http.Headers;

    public static class ContentType
    {
        public static MediaTypeHeaderValue Json => new MediaTypeHeaderValue("application/json");
        public static MediaTypeHeaderValue Form => new MediaTypeHeaderValue("application/x-www-form-urlencoded");
    }
}