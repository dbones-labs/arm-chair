namespace ArmChair.Http
{
    /// <summary>
    /// Auth mechanism to be applied on the http connection
    /// </summary>
    public interface IAuthentication
    {
        void Apply(IRequest request);
    }
}