using System.Collections.Generic;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class IdpSSOMetadataConfiguration : MetadataConfiguration, IIdpSSOMetadataConfiguration
    {
        public bool WantAuthnRequestsSigned { get; set; }

        public IEnumerable<ISingleSignOnServiceContext> SingleSignOnServices { get; set; }
    }
}
