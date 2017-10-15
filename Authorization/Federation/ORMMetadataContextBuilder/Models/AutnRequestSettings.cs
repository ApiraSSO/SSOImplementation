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
        public bool Version { get; set; }
        public RequitedAutnContext RequitedAutnContext { get; set; }
        
    }
}