using System;
using System.IdentityModel.Metadata;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class SPSSOMetadataProvider : MetadataGeneratorBase, ISPMetadataGenerator
    {
        public SPSSOMetadataProvider(IFederationMetadataWriter metadataWriter, ICertificateManager certificateManager, IMetadataSerialiser<MetadataBase> serialiser, Func<MetadataGenerateRequest, FederationPartyContext> configuration)
            :base(metadataWriter, certificateManager, serialiser, configuration)
        { }
    }
}