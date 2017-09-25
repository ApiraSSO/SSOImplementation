using System;
using System.Linq;
using ComponentSpace.Saml2.Metadata;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.Signing.Xml;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public class SPSSOMetadataProvider : MetadataGeneratorBase<SpSsoDescriptor>
    {
        public SPSSOMetadataProvider(IFederationMetadataWriter metadataWriter, ICertificateManager certificateManager, IXmlSignatureManager xmlSignatureManager)
            : base(metadataWriter, certificateManager, xmlSignatureManager)
        { }

        protected override RoleDescriptorType GetDescriptor(IMetadataConfiguration configuration)
        {
            var spConfiguration = configuration as ISPSSOMetadataConfiguration;

            if (spConfiguration == null)
                throw new InvalidCastException(string.Format("Expected type: {0} but was: {1}", typeof(SPSSOMetadataConfiguration).Name, configuration.GetType().Name));

            var descriptor = new SpSsoDescriptor
            {
                ID = configuration.DescriptorId,
                AuthnRequestsSigned = spConfiguration.AuthnRequestsSigned,
                ProtocolSupportEnumeration = spConfiguration.SupportedProtocols.ToArray()[0]
            };

            foreach (var cs in spConfiguration.ConsumerServices)
            {
                var consumerService = new AssertionConsumerService();
                consumerService.Index = cs.Index;
                consumerService.Location = cs.Location;

                consumerService.Binding = cs.Binding;

                descriptor.AssertionConsumerServices.Add(consumerService);
            }

            return descriptor;
        }

        protected override Action<EntityDescriptor, SpSsoDescriptor> AssignmentAction
        {
            get
            {
                return (ed, d) => ed.SpSsoDescriptors.Add(d);
            }
        }
    }
}