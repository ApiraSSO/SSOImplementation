using System.Collections.Generic;
using Kernel.Federation.MetaData.Configuration.EndPoint;

namespace Kernel.Federation.MetaData.Configuration.RoleDescriptors
{
    public class SPSSODescriptorConfiguration : SSODescriptorConfiguration
    {
        public bool AuthenticationRequestsSigned { get; set; }

        public bool WantAssertionsSigned { get; set; }
        public ICollection<IndexedEndPointConfiguration> AssertionConsumerServices { get; }
        public SPSSODescriptorConfiguration()
        {
            this.WantAssertionsSigned = true;
            this.AuthenticationRequestsSigned = true;
            this.AssertionConsumerServices = new List<IndexedEndPointConfiguration>();
        }
    }
}