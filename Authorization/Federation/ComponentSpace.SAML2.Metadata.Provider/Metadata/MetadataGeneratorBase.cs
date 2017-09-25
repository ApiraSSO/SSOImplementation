using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ComponentSpace.Saml2.Metadata;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.Signing.Xml;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public abstract class MetadataGeneratorBase<T> : IMetadataGenerator where T : RoleDescriptorType
    {
        protected IFederationMetadataWriter _federationMetadataWriter;

        protected ICertificateManager _certificateManager;
        protected IXmlSignatureManager _xmlSignatureManager;

        protected abstract Action<EntityDescriptor, T> AssignmentAction { get; }

        public MetadataGeneratorBase(IFederationMetadataWriter federationMetadataWriter, ICertificateManager certificateManager, IXmlSignatureManager xmlSignatureManager)
        {
            this._federationMetadataWriter = federationMetadataWriter;
            this._certificateManager = certificateManager;
            this._xmlSignatureManager = xmlSignatureManager;
        }

        public Task CreateMetadata(IMetadataConfiguration configuration)
        {
            try
            {
                var descriptor = GetDescriptor(configuration);

                ProcessKeys(configuration, descriptor);

                var entityDescriptor = BuildEntityDesciptor(configuration, descriptor);

                var xMetadata = entityDescriptor.ToXml();
                var xmldoc = new XmlDocument();
                xmldoc.Load(xMetadata.CreateReader());
                var metadata = xmldoc.DocumentElement;
                SignMetadata(configuration, metadata);

                _federationMetadataWriter.Write(metadata, configuration);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ProcessKeys(IMetadataConfiguration configuration, RoleDescriptorType Descriptor)
        {
            foreach (var k in configuration.Keys)
            {
                var certificate = _certificateManager.GetCertificate(k.SertificateFilePath, k.CertificatePassword);

                var keyDescriptor = new KeyDescriptor();

                keyDescriptor.Use = k.Usage;

                var keyInfo = _xmlSignatureManager.CreateKeyInfo(certificate);
                var xElement = keyInfo.GetXml().OuterXml;
                keyDescriptor.KeyInfo = XElement.Parse(xElement);

                Descriptor.KeyDescriptors.Add(keyDescriptor);
            }
        }

        protected void SignMetadata(IMetadataConfiguration configuration,  XmlElement xml)
        {
            var signMetadataKey = configuration.Keys.Where(k => k.DefaultForMetadataSigning)
                    .FirstOrDefault();

            if (signMetadataKey == null)
                throw new Exception("No default certificate found");

            var certificate = _certificateManager.GetCertificate(signMetadataKey.SertificateFilePath, signMetadataKey.CertificatePassword);
    
            this._xmlSignatureManager.Generate(xml, certificate.PrivateKey, null, certificate, null, null, null);
        }

        protected virtual EntityDescriptor BuildEntityDesciptor(IMetadataConfiguration configuration, RoleDescriptorType descriptor)
        {
            var entityDescriptor = new EntityDescriptor()
            {
                EntityID = new EntityIDType { Uri = configuration.EntityId.AbsoluteUri },
                ID = "84CCAA9F05EE4BA1B13F8943FDF1D320"
            };

            AssignmentAction(entityDescriptor, (T)descriptor);

            return entityDescriptor;
        }

        protected abstract RoleDescriptorType GetDescriptor(IMetadataConfiguration configuration);

        public Task CreateMetadata()
        {
            throw new NotImplementedException();
        }
    }
}