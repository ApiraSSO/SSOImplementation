using System;
using System.Linq;
using System.IdentityModel.Metadata;
using Kernel.Federation.MetaData;

namespace Federation.Metadata.RelyingParty.Handlers
{
    internal class MetadataEntitiesDescriptorHandler : IMetadataHandler<EntitiesDescriptor>
    {
        public Uri ReadIdpLocation(EntitiesDescriptor metadata, Uri binding)
        {
            
            var idDescpritor = metadata.ChildEntities.SelectMany(x => x.RoleDescriptors)
                .First(x => x.GetType() == typeof(IdentityProviderSingleSignOnDescriptor)) as IdentityProviderSingleSignOnDescriptor;
            var signInUrl = idDescpritor.SingleSignOnServices.FirstOrDefault(x => x.Binding == binding)
                .Location;

            return signInUrl;
        }
    }
}