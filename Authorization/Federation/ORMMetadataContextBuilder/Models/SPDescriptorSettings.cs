using System.Collections.Generic;

namespace ORMMetadataContextProvider.Models
{
    public class SPDescriptorSettings : SSODescriptorSettings
    {
        public SPDescriptorSettings()
        {
            this.AssertionServices = new List<IndexedEndPointSetting>();
        }
        public bool RequestSigned { get; set; }
        public bool WantAssertionsSigned { get; set; }
        public virtual ICollection<IndexedEndPointSetting> AssertionServices { get; }
        
    }
}