namespace Todo.Service.Infrastructure.Configuration
{
    public class WebServerConfig
    {
        public bool IsSslEnabled { get; set; } = false;
        public string Domain { get; set; } = "localhost";
        public int Port { get; set; } = 80;
    }
}