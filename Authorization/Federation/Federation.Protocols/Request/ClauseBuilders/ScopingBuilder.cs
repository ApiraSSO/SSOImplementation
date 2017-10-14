using Kernel.Federation.FederationPartner;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class ScopingBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, AuthnRequestConfiguration configuration)
        {
            request.Scoping = new Scoping
            {
                ProxyCount = "0",
                RequesterId = new[] { configuration.EntityId }
            };
        }
    }
}