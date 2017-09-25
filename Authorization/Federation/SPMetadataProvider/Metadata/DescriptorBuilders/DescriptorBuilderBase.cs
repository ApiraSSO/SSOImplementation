using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;
using WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders
{
    internal abstract class DescriptorBuilderBase<TRole> : IDescriptorBuilder<TRole> where TRole : RoleDescriptor
    {
        public TRole BuildDescriptor(RoleDescriptorConfiguration configuration)
        {
            var descriptor = this.BuildDescriptorInternal(configuration);
            descriptor = this.BuildAll(descriptor, configuration);
            return descriptor;
        }
        
        private TRole BuildAll(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration)
        {
            var builders = MemberBuilderFactory.GetBuilders();
            builders.Aggregate(descriptor, (d, next) =>
            {
                next.Build(descriptor, configuration);
                return descriptor;
            });
            return (TRole)descriptor;
        }
        
        protected abstract TRole BuildDescriptorInternal(RoleDescriptorConfiguration configuration);
    }
}