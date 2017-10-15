using System.Collections.Generic;
using Kernel.Data;
using Shared.Federtion.Models;

namespace ORMMetadataContextProvider.Models
{
    public class SamlAutnContext : BaseModel
    {
        public SamlAutnContext()
        {
            this.RequitedAutnContexts = new List<RequitedAutnContext>();
        }
        public AuthnContextType RefType { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RequitedAutnContext> RequitedAutnContexts { get; }
    }
}