using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class RequitedAutnContext : BaseModel
    {
        public RequitedAutnContext()
        {
            this.RequitedAuthnContexts = new List<AutnContext>();
        }

        public string Comparision { get; set; }
        public virtual ICollection<AutnContext> RequitedAuthnContexts { get; }
    }
}