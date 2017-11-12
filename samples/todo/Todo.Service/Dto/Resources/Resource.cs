namespace Todo.Service.Dto.Resources
{
    using System.Collections.Generic;

    /// <summary>
    /// the base of a resource class, do not inherit of this directly
    /// </summary>
    public abstract class ResourceBase
    {
        public Dictionary<string, string> Links { get; set; }
        public Dictionary<string, string> Actions { get; set; }
    }

    /// <summary>
    /// a REST resource, inherit of this for each resource
    /// </summary>
    /// <remarks>
    /// based off the Rancher spec
    /// https://github.com/rancher/api-spec/blob/master/specification.md#resources
    /// </remarks>
    public abstract class Resource : ResourceBase
    {
        public string Id { get; set; }
        public string Revision { get; set; }

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


    /// <summary>
    /// a collection of resources.
    /// </summary>
    /// <typeparam name="T">the type of the resource</typeparam>
    public class CollectionResource<T> : ResourceBase where T : Resource
    {
        public List<T> Data { get; set; }
    }

    
}