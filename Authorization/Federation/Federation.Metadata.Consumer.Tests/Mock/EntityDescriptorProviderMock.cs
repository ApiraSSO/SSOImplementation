using System;
using System.IdentityModel.Metadata;
using System.Security.Cryptography.X509Certificates;
using Shared.Federtion.Constants;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    public class EntityDescriptorProviderMock
    {
        public static EntityDescriptor GetEntityDescriptor()
        {
            var descriptor = new EntityDescriptor();
            var cert = EntityDescriptorProviderMock.GetMockCertificate();
            var idpRole = new IdentityProviderSingleSignOnDescriptor();
            idpRole.SingleSignOnServices.Add(new ProtocolEndpoint(new Uri(ProtocolBindings.HttpRedirect), new Uri("http://localhost:60879")));
            descriptor.RoleDescriptors.Add(idpRole);
            return descriptor;
        }

        public static EntitiesDescriptor GetEntitiesDescriptor()
        {
            var descriptor = new EntitiesDescriptor();
            var entityDesc = EntityDescriptorProviderMock.GetEntityDescriptor();
            descriptor.ChildEntities.Add(entityDesc);
            return descriptor;
        }

        public static X509Certificate2 GetMockCertificate()
        {
            var store = new X509Store(StoreName.My);
            store.Open(OpenFlags.ReadOnly);
            var certSource = store.Certificates[0];
            var rawData = certSource.GetRawCertData();
            var cert = new X509Certificate2(rawData);
            store.Close();
            return cert;
        }
    }
}