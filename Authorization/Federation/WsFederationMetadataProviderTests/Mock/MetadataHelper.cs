using System;
using System.IdentityModel.Metadata;
using System.Security.Cryptography.X509Certificates;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using Kernel.Federation.MetaData.Configuration.EndPoint;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Kernel.Federation.MetaData.Configuration.Organisation;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProviderTests.Mock
{
    internal class MetadataHelper
    {
        public static EntityDesriptorConfiguration BuildEntityDesriptorConfiguration()
        {
            var federationId = String.Format("{0}_{1}", "flowz", Guid.NewGuid());
            var entityDescriptorConfiguration = new EntityDesriptorConfiguration
            {
                CacheDuration = TimeSpan.FromDays(100),
                EntityId = "Imperial.flowz.co.uk",
                Id = federationId,
                ValidUntil = new DateTimeOffset(DateTime.Now.AddDays(30)),
                Organisation = MetadataHelper.BuikdOrganisationConfiguration()
            };
            return entityDescriptorConfiguration;
        }

        public static OrganisationConfiguration BuikdOrganisationConfiguration()
        {
            return new OrganisationConfiguration
            {
                Name = "Apira LTD",
                DisplayName = "Flowz",
                OrganisationContacts = new ContactConfiguration(),
            };
        }
        public static KeyDescriptorConfiguration BuildKeyDescriptorConfiguration()
        {
            var certificateContext = new CertificateContext
            {
                StoreName = "TestCertStore",
                SearchCriteria = "ApiraTestCertificate",
                ValidOnly = false,
                SearchCriteriaType = X509FindType.FindBySubjectName,
                StoreLocation = StoreLocation.LocalMachine
            };
            
            var keyDescriptorConfiguration = new KeyDescriptorConfiguration
            {
                IsDefault = true,
                Use = KeyUsage.Signing,
                CertificateContext = certificateContext
            };
            return keyDescriptorConfiguration;
        }

        public static SPSSODescriptorConfiguration BuildSPSSODescriptorConfiguration()
        {
            var sPSSODescriptorConfiguration = new SPSSODescriptorConfiguration
            {
                WantAssertionsSigned = true,
                ValidUntil = new DateTimeOffset(DateTime.Now.AddDays(100)),
                Organisation = MetadataHelper.BuikdOrganisationConfiguration(),
                AuthenticationRequestsSigned = true,
                CacheDuration = TimeSpan.FromDays(100),
                RoleDescriptorType = typeof(ServiceProviderSingleSignOnDescriptor)
            };
            //supported protocols
            sPSSODescriptorConfiguration.ProtocolSupported.Add(new Uri("urn:oasis:names:tc:SAML:2.0:protocol"));
            //key descriptors
            var keyDescriptorConfiguration = MetadataHelper.BuildKeyDescriptorConfiguration();
            sPSSODescriptorConfiguration.KeyDescriptors.Add(keyDescriptorConfiguration);

            //assertinon service
            var indexedEndPointConfiguration = new IndexedEndPointConfiguration
            {
                Index = 0,
                IsDefault = true,
                Binding = new Uri("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST"),
                Location = new Uri("http://localhost:60879/api/Account/SSOLogon")
            };
            sPSSODescriptorConfiguration.AssertionConsumerServices.Add(indexedEndPointConfiguration);

            return sPSSODescriptorConfiguration;
        }
    }
}