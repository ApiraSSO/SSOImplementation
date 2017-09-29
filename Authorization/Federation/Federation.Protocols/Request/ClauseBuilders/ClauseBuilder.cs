using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Kernel.Federation.Protocols;
using Kernel.Federation.FederationPartner;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal abstract class ClauseBuilder : IAuthnRequestClauseBuilder<AuthnRequest>
    {
        public void Build(AuthnRequest request, FederationPartnerContext relyingParty)
        {
            var metadataContext = relyingParty.MetadataContext;

            var entityDescriptor = metadataContext.EntityDesriptorConfiguration;

            this.BuildInternal(request, entityDescriptor);
        }

        protected abstract void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor);
    }
}