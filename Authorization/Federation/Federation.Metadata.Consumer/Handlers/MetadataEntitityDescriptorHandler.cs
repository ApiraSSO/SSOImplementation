using System;
using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Federation.MetaData;

namespace Federation.Metadata.RelyingParty.Handlers
{
    internal class MetadataEntitityDescriptorHandler : IMetadataHandler<EntityDescriptor>
    {
        public Uri ReadIdpLocation(EntityDescriptor metadata, Uri binding)
        {
            var signInUrl = metadata.RoleDescriptors.OfType<IdentityProviderSingleSignOnDescriptor>()
                .SelectMany(x => x.SingleSignOnServices).
                First(x => x.Binding == binding).Location;

            return signInUrl;
        }
    }
}