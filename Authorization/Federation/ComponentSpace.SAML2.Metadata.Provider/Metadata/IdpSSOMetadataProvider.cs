using System;
using System.Linq;
using ComponentSpace.Saml2.Metadata;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.Signing.Xml;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public class IdpSSOMetadataProvider : MetadataGeneratorBase<IdpSsoDescriptor>
    {
        public IdpSSOMetadataProvider(IFederationMetadataWriter metadataWriter, ICertificateManager certificateManager, IXmlSignatureManager xmlSignatureManager)
            : base(metadataWriter, certificateManager, xmlSignatureManager)
        {

        }

        protected override RoleDescriptorType GetDescriptor(IMetadataConfiguration configuration)
        {
            var idpConfiguration = configuration as IIdpSSOMetadataConfiguration;

            if (idpConfiguration == null)
                throw new InvalidCastException(string.Format("Expected type: {0} but was: {1}", typeof(IdpSSOMetadataConfiguration).Name, configuration.GetType().Name));

            var descriptor = new IdpSsoDescriptor
            {
                ID = configuration.DescriptorId,
                WantAuthnRequestsSigned = idpConfiguration.WantAuthnRequestsSigned,
                ProtocolSupportEnumeration = idpConfiguration.SupportedProtocols.ToArray()[0]
            };

            
            foreach (var sso in idpConfiguration.SingleSignOnServices)
            {
                var singleSignOnService = new SingleSignOnService()
                {
                    Location = sso.Location,

                    Binding = sso.Binding
                };

                descriptor.SingleSignOnServices.Add(singleSignOnService);
            }

            return descriptor;
        }

        protected override Action<EntityDescriptor, IdpSsoDescriptor> AssignmentAction
        {
            get { return (ed, d) => ed.IdpSsoDescriptors.Add(d); }
        }
    }
}