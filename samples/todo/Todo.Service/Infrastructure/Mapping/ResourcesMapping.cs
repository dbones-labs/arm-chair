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
            
            CreateMap<TodoItem, TodoResource>()
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => GetLinksFromTask(src)))
                .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => GetActionsFromTask(src)));
            
            CreateMap<IEnumerable<TodoItem>, CollectionResource<TodoResource>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => GetCollectionLinksFromTask(src)))
                .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => GetCollectionActionsFromTask(src)));

        }

        protected IDictionary<string, string> GetLinksFromTask(TodoItem todoItem)
        {
            var result = new Dictionary<string,string>();
            result.Add("collection", $"{baseUrl}/");
            return result;
        }
        
        
        protected IDictionary<string, string> GetActionsFromTask(TodoItem todoItem)
        {
            var result = new Dictionary<string,string>();
            if (!todoItem.IsComplete) result.Add("update", $"{baseUrl}/{todoItem.Id}");
            if (!todoItem.IsComplete) result.Add("complete", $"{baseUrl}/{todoItem.Id}/complete");
            result.Add("remove", $"{baseUrl}/{todoItem.Id}");
            return result;
        }
        
        
        protected IDictionary<string, string> GetCollectionLinksFromTask(IEnumerable<TodoItem> task)
        {
            var result = new Dictionary<string,string>();
            result.Add("self", $"{baseUrl}/");
            result.Add("filterByActive", $"{baseUrl}/active");
            result.Add("filterByPriority", $"{baseUrl}/priority");
            return result;
        }
        
        
        protected IDictionary<string, string> GetCollectionActionsFromTask(IEnumerable<TodoItem> task)
        {
            var result = new Dictionary<string,string>();
            result.Add("createTodoTask", $"{baseUrl}/");
            result.Add("prune", $"{baseUrl}/prune");
            return result;
        }
    }
}