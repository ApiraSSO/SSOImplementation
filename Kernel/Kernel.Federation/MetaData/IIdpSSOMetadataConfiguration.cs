using System.Collections.Generic;

namespace Kernel.Federation.MetaData
{
    public interface IIdpSSOMetadataConfiguration : IMetadataConfiguration
    {
        bool WantAuthnRequestsSigned { get; set; }

        IEnumerable<ISingleSignOnServiceContext> SingleSignOnServices { get; set; }
    }
}