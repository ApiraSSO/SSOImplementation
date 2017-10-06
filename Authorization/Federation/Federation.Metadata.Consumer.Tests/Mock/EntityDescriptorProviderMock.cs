using System.IdentityModel.Metadata;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    public class EntityDescriptorProviderMock
    {
        public static EntityDescriptor GetEntityDescriptor()
        {
            var descriptor = new EntityDescriptor();
            var idpRole = new IdentityProviderSingleSignOnDescriptor();
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