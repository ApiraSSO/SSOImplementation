using System;
using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders
{
    internal class ProtocolsSupportedBuilder : RoleDescriptorMemberBuilder
    {
        protected override void BuildInternal(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration)
        {
            if (configuration.ProtocolSupported == null)
                throw new ArgumentNullException("protocolSupported");

            configuration.ProtocolSupported.Aggregate(descriptor.ProtocolsSupported, (t, next) =>
            {
                t.Add(next);
                return t;
            });
        }
    }
}