using System;
using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Federation.MetaData;

namespace Federation.Metadata.FederationPartner.Handlers
{
    internal class MetadataEntitiesDescriptorHandler : IMetadataHandler<EntitiesDescriptor>
    {
        public Uri ReadIdpLocation(EntitiesDescriptor metadata, Uri binding)
        {
            
            var signInUrl = metadata.ChildEntities.SelectMany(x => x.RoleDescriptors)
                .OfType<IdentityProviderSingleSignOnDescriptor>()
               .SelectMany(x => x.SingleSignOnServices).
                First(x => x.Binding == binding).Location;

            return signInUrl;
        }
    }
}