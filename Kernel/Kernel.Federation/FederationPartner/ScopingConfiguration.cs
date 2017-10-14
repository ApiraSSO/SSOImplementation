using System.Collections.Generic;
using System.Linq;

namespace Kernel.Federation.FederationPartner
{
    public class ScopingConfiguration
    {
        public ScopingConfiguration(params string[] entityIds)
        {
            this.PoxyCount = 0;
            this.RequesterIds = new List<string>();
            if (entityIds != null)
                entityIds.Aggregate(this.RequesterIds, (t, next) => { t.Add(next); return t; });
        }
        public uint PoxyCount { get; set; }
        public ICollection<string> RequesterIds { get; }
    }
}