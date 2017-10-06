using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Federation.MetaData;

namespace Federation.Metadata.FederationPartner.Handlers
{
    internal class MetadataEntitityDescriptorHandler : IMetadataHandler<EntityDescriptor>
    {
        public IEnumerable<TRole> GetRoleDescriptors<TRole>(EntityDescriptor metadata)
        {
            return metadata.RoleDescriptors.OfType<TRole>();
        }

        public Uri ReadIdpLocation(EntityDescriptor metadata, Uri binding)
        {
            var signInUrl = metadata.RoleDescriptors.OfType<IdentityProviderSingleSignOnDescriptor>()
                .SelectMany(x => x.SingleSignOnServices).
                First(x => x.Binding == binding).Location;

            return signInUrl;
        }
    }
}