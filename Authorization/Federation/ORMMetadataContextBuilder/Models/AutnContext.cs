using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class AutnContext : BaseModel
    {
        public AutnContext()
        {
            this.RequitedAutnContexts = new List<RequitedAutnContext>();
        }
        public string Value { get; }
        public string Description { get; set; }
        public ICollection<RequitedAutnContext> RequitedAutnContexts { get; }
    }
}