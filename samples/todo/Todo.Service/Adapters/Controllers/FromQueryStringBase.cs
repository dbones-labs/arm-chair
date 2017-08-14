namespace Todo.Service.Adapters.Controllers
{
    public class FromQueryStringBase
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }
}