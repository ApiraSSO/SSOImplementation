using System;
using System.IdentityModel.Metadata;
using Shared.Federtion.Constants;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    public class EntityDescriptorProviderMock
    {
        public static EntityDescriptor GetEntityDescriptor()
        {
            var descriptor = new EntityDescriptor();
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
    }
}