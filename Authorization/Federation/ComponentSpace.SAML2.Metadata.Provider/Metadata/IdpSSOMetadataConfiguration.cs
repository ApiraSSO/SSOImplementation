using System.Collections.Generic;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public class IdpSSOMetadataConfiguration : MetadataConfiguration, IIdpSSOMetadataConfiguration
    {
        public bool WantAuthnRequestsSigned { get; set; }

        public IEnumerable<ISingleSignOnServiceContext> SingleSignOnServices { get; set; }
    }
}