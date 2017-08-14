namespace Todo.Service.Dto.Resources
{
    using System.Collections.Generic;

    /// <summary>
    /// a REST resource, base class
    /// </summary>
    /// <remarks>
    /// based off the Rancher spec
    /// https://github.com/rancher/api-spec/blob/master/specification.md#resources
    /// </remarks>
    public abstract class Resource
    {
        public string Id { get; set; }
        public string Revision { get; set; }
        public Dictionary<string, string> Links { get; set; }
        public Dictionary<string, string> Actions { get; set; }
    }

    
    public class CollectionResource<T> : Resource where T : Resource
    {
        public List<T> Data { get; set; }
    }

    /*   
    {
        "id":      "b1b2e7006be", 
        "type":    "file",
        "links":   {  see links  },
        "actions": { see actions  },
        "name":    "ultimate_answer.txt",
        "size":    2,
        .... more stuff
    }
*/
}