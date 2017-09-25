using System;
using System.IdentityModel.Metadata;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration;

namespace WsFederationMetadataProvider.Metadata
{
    public class SPSSOMetadataProvider : MetadataGeneratorBase, ISPMetadataGenerator
    {
        public SPSSOMetadataProvider(IFederationMetadataWriter metadataWriter, ICertificateManager certificateManager, IMetadataSerialiser<MetadataBase> serialiser, Func<MetadataType, MetadataContext> configuration)
            :base(metadataWriter, certificateManager, serialiser, configuration)
        { }
    }
}