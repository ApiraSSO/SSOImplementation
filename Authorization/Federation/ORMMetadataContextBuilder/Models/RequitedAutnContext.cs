using System.Collections.Generic;
using Kernel.Data;
using Shared.Federtion.Models;

namespace ORMMetadataContextProvider.Models
{
    public class RequitedAutnContext : BaseModel
    {
        public RequitedAutnContext()
        {
            this.RequitedAuthnContexts = new List<SamlAutnContext>();
        }

        public AuthnContextComparisonType Comparison { get; set; }
        public virtual ICollection<SamlAutnContext> RequitedAuthnContexts { get; }
    }
}