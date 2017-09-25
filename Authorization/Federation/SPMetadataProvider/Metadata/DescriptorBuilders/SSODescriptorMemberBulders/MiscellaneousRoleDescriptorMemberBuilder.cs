using System.IdentityModel.Metadata;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders
{
    internal class MiscellaneousRoleDescriptorMemberBuilder : RoleDescriptorMemberBuilder
    {
        protected override void BuildInternal(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration)
        {

            descriptor.ErrorUrl = configuration.ErrorUrl;
            descriptor.ValidUntil = configuration.ValidUntil.DateTime;
        }
    }
}