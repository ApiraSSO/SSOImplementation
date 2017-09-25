using System;
using System.IdentityModel.Metadata;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders
{
    internal abstract class RoleDescriptorMemberBuilder
    {
        public virtual void Build(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration)
        {
            if (descriptor == null)
                throw new ArgumentNullException("descriptor");
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            this.BuildInternal(descriptor, configuration);
        }

        protected abstract void BuildInternal(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration);
    }
}