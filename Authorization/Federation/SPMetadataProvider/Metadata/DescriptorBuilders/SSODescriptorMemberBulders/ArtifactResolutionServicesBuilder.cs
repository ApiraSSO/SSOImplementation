using System;
using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders
{
    internal class ArtifactResolutionServicesBuilder : RoleDescriptorMemberBuilder
    {
        protected override void BuildInternal(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration)
        {
            var sSODescriptorConfiguration = configuration as SSODescriptorConfiguration;
            if (sSODescriptorConfiguration == null)
                throw new InvalidOperationException(String.Format("Configuration type expected: {0}.", typeof(SSODescriptorConfiguration).Name));
            if (sSODescriptorConfiguration.ArtifactResolutionServices == null)
                throw new ArgumentNullException("crtifactResolutionServices");

            sSODescriptorConfiguration.ArtifactResolutionServices.Aggregate(descriptor, (d, next) =>
            {
                ((SingleSignOnDescriptor)d).ArtifactResolutionServices.Add(next.Index, new IndexedProtocolEndpoint(next.Index, next.Binding, next.Location));
                return d;
            });
        }
    }
}