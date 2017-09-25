using System;
using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders
{
    internal class SingleLogoutServicesBuilder : RoleDescriptorMemberBuilder
    {
        protected override void BuildInternal(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration)
        {
            var sSODescriptorConfiguration = configuration as SSODescriptorConfiguration;
            if (sSODescriptorConfiguration == null)
                throw new InvalidOperationException(String.Format("Configuration type expected: {0}.", typeof(SSODescriptorConfiguration).Name));

            if (sSODescriptorConfiguration.SingleLogoutServices == null)
                throw new ArgumentNullException("singleLogoutServices");
            sSODescriptorConfiguration.SingleLogoutServices.Aggregate(descriptor, (d, next) =>
            {
                ((SingleSignOnDescriptor)d).SingleLogoutServices.Add(new ProtocolEndpoint(next.Binding, next.Location));
                return d;
            });
        }
    }
}