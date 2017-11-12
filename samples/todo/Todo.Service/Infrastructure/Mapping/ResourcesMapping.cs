namespace Todo.Service.Infrastructure.Mapping
{
    using System.Collections.Generic;
    using Configuration;
    using Dto.Resources;
    using Microsoft.Extensions.Configuration;
    using Models;

    public class ResourcesMapping : AutoMapper.Profile
    {
        private readonly string baseUrl;
        
        public ResourcesMapping(IConfigurationRoot config)
        {
            var webServerConfig = config.GetSection("WebServer").Get<WebServerConfig>();

            var http = webServerConfig.IsSslEnabled ? "https://" : "http://";
            var domain = webServerConfig.Domain;
            var port = webServerConfig.Port == 80 ? "" : $":{webServerConfig.Port}";
            baseUrl = $"{http}{domain}{port}/api/v1/tasks";
            
            CreateMap<Task, TodoResource>()
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => GetLinksFromTask(src)))
                .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => GetActionsFromTask(src)));
            
            CreateMap<IEnumerable<Task>, CollectionResource<TodoResource>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => GetCollectionLinksFromTask(src)))
                .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => GetCollectionActionsFromTask(src)));

        }

        protected IDictionary<string, string> GetLinksFromTask(Task task)
        {
            var result = new Dictionary<string,string>();
            result.Add("collection", $"{baseUrl}/");
            return result;
        }
        
        
        protected IDictionary<string, string> GetActionsFromTask(Task task)
        {
            var result = new Dictionary<string,string>();
            if (!task.IsComplete) result.Add("update", $"{baseUrl}/{task.Id}");
            if (!task.IsComplete) result.Add("complete", $"{baseUrl}/{task.Id}/complete");
            result.Add("remove", $"{baseUrl}/{task.Id}");
            return result;
        }
        
        
        protected IDictionary<string, string> GetCollectionLinksFromTask(IEnumerable<Task> task)
        {
            var result = new Dictionary<string,string>();
            result.Add("self", $"{baseUrl}/");
            result.Add("filterByActive", $"{baseUrl}/active");
            result.Add("filterByPriority", $"{baseUrl}/priority");
            return result;
        }
        
        
        protected IDictionary<string, string> GetCollectionActionsFromTask(IEnumerable<Task> task)
        {
            var result = new Dictionary<string,string>();
            result.Add("createTodoTask", $"{baseUrl}/");
            result.Add("prune", $"{baseUrl}/prune");
            return result;
        }
    }
}