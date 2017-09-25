using System;
using ComponentSpace.SAML2.Metadata.Provider.CertificateProviderImplementation;
using ComponentSpace.SAML2.Metadata.Provider.Metadata;
using ComponentSpace.SAML2.Metadata.Provider.Tests.Mock;
using Kernel.Extensions;
using NUnit.Framework;
using SecurityManagement;

namespace ComponentSpace.SAML2.Metadata.Provider.Tests
{
    [TestFixture]
    public class SPMetadataTests
    {
        [Test]
        public void SPMetadataProviderTest()
        {
            var result = String.Empty;
            var metadataWriter = new TestMetadatWriter(el => result = el.OuterXml);

            //var metadataWriter = new SSOMetadataFileWriter();

            var ssoCryptoProvider = new CertificateManager();
            var xmlSignatureManager = new XmlSignatureManager();

            var sPSSOMetadataProvider = new SPSSOMetadataProvider(metadataWriter, ssoCryptoProvider, xmlSignatureManager);

            var configuration = new SPSSOMetadataConfiguration
            {
                AuthnRequestsSigned = true,
                DescriptorId = "Idp1",
                EntityId = new Uri("http://localhost:64247/sso/saml2/post/AssertionConsumerService.aspx"),
                MetadatFilePathDestination = @"D:\SPSSOMetadata.xml",
                SupportedProtocols = new[] { "urn:oasis:names:tc:SAML:2.0:protocol" },
                SignMetadata = true,
                ConsumerServices = new ConsumerServiceContext[]{new ConsumerServiceContext
                {
                    Index = 0,
                    IsDefault = true,
                    Binding = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST",
                    Location = "http://localhost:64247/sso/saml2/post/AssertionConsumerService.aspx"
                }},
                Keys = new CertificateContext[] { new CertificateContext
                {
                    SertificateFilePath = @"D:\Dan\Software\SGWI\ThirdParty\devCertsPackage\employeeportaldev.safeguardworld.com.pfx",
                    CertificatePassword = StringExtensions.ToSecureString("$Password1!"),
                    Usage = "signing",
                    DefaultForMetadataSigning = true
                }}
            };

            sPSSOMetadataProvider.CreateMetadata(configuration);
        }
    }
}