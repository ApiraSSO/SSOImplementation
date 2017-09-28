using System;
using System.Linq;
using System.IdentityModel.Metadata;
using Kernel.Federation.MetaData;

namespace Federation.Metadata.RelyingParty.Handlers
{
    internal class MetadataEntitityDescriptorHandler : IMetadataHandler<EntityDescriptor>
    {
        public Uri ReadIdpLocation(EntityDescriptor metadata, Uri binding)
        {
            var idDescpritor = metadata.RoleDescriptors.Select(x => x)
                .First(x => x.GetType() == typeof(IdentityProviderSingleSignOnDescriptor)) as IdentityProviderSingleSignOnDescriptor;
            var signInUrl = idDescpritor.SingleSignOnServices.First(x => x.Binding == binding).Location;
            return signInUrl;
        }
    }
}