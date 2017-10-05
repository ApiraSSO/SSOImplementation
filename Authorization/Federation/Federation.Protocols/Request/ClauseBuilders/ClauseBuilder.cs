using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Kernel.Federation.Protocols;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal abstract class ClauseBuilder : IAuthnRequestClauseBuilder<AuthnRequest>
    {
        public void Build(AuthnRequest request, FederationPartyContext federationParty)
        {
            var metadataContext = federationParty.MetadataContext;

            var entityDescriptor = metadataContext.EntityDesriptorConfiguration;

            this.BuildInternal(request, entityDescriptor);
        }

        protected abstract void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor);
    }
}