using System.Collections.Generic;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class SPSSOMetadataConfiguration : MetadataConfiguration, ISPSSOMetadataConfiguration
    {
        public bool AuthnRequestsSigned { get; set; }

        public IEnumerable<IConsumerServiceContext> ConsumerServices { get; set; }
    }    
}
