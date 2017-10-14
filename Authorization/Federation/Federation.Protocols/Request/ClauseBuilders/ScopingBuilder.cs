using System.Linq;
using Kernel.Federation.FederationPartner;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class ScopingBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, AuthnRequestConfiguration configuration)
        {
            if (configuration.ScopingConfiguration == null)
                return;
            if (configuration.ScopingConfiguration.RequesterIds != null && configuration.ScopingConfiguration.RequesterIds.Count == 0)
                configuration.ScopingConfiguration.RequesterIds.Add(configuration.EntityId);
            request.Scoping = new Scoping
            {
                ProxyCount = configuration.ScopingConfiguration.PoxyCount.ToString(),
                RequesterId = configuration.ScopingConfiguration.RequesterIds.ToArray()
            };
        }
    }
}