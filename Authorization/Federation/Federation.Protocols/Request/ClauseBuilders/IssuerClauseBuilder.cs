using Kernel.Federation.FederationPartner;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class IssuerClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, AuthnRequestConfiguration configuration)
        {
            request.Issuer = new NameId
            {
                Value = configuration.EntityId,
                Format = NameIdentifierFormats.Entity
            };
        }
    }
}