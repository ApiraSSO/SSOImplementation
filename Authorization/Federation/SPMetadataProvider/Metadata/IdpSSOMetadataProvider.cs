using System;
using System.IdentityModel.Metadata;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.Signing.Xml;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class IdpSSOMetadataProvider : MetadataGeneratorBase, IIdMetadataGenerator
    {
        public IdpSSOMetadataProvider(IFederationMetadataWriter metadataWriter, ICertificateManager certificateManager, IXmlSignatureManager xmlSignatureManager, IMetadataSerialiser<MetadataBase> serialiser, Func<IMetadataGenerator, IMetadataConfiguration> configuration)
            : base(metadataWriter, certificateManager, xmlSignatureManager, serialiser, configuration)
        {
           
        }
    }
}