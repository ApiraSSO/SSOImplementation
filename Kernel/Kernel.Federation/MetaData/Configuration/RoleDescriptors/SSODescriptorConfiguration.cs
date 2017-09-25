using System;
using System.Collections.Generic;
using Kernel.Federation.MetaData.Configuration.EndPoint;

namespace Kernel.Federation.MetaData.Configuration.RoleDescriptors
{
    public class SSODescriptorConfiguration : RoleDescriptorConfiguration
    {
        public ICollection<Uri> NameIdentifierFormats { get; }
        public ICollection<IndexedEndPointConfiguration> ArtifactResolutionServices { get; }
        public ICollection<EndPointConfiguration> SingleLogoutServices { get; }
        public SSODescriptorConfiguration()
        {
            this.ArtifactResolutionServices = new List<IndexedEndPointConfiguration>();
            this.NameIdentifierFormats = new List<Uri>();
            this.SingleLogoutServices = new List<EndPointConfiguration>();
        }
    }
}