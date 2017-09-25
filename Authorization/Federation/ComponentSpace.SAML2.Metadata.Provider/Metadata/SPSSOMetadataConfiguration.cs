using System.Collections.Generic;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public class SPSSOMetadataConfiguration : MetadataConfiguration, ISPSSOMetadataConfiguration
    {
        public bool AuthnRequestsSigned { get; set; }

        public IEnumerable<IConsumerServiceContext> ConsumerServices { get; set; }
    }    
}