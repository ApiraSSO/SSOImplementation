using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class AutnRequestSettings : BaseModel
    {
        public AutnRequestSettings()
        {
        }
        public bool IsPassive { get; set; }
        public bool ForceAuthn { get; set; }
        public string Version { get; set; }
        public virtual NameIdConfiguration NameIdConfiguration { get; set; }
        public virtual RequitedAutnContext RequitedAutnContext { get; set; }
    }
}