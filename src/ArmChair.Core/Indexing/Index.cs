namespace ArmChair
{
    using System.Collections.Generic;

    public class Index
    {
        public Index()
        {
            Fields = new List<IDictionary<string, Order>>();
        }

        public IList<IDictionary<string,Order>> Fields { get; set; }

        public virtual void Add(string name, Order sort = Order.Asc)
        {
            Fields.Add(new Dictionary<string, Order>(){
            {
                name, sort
            }});
        }
    }
}