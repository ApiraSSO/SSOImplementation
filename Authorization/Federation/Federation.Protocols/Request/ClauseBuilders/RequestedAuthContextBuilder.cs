using Kernel.Federation.FederationPartner;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class RequestedAuthContextBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, AuthnRequestConfiguration configuration)
        {
            request.RequestedAuthnContext = new RequestedAuthnContext
            {
                Comparison = AuthnContextComparisonType.Exact,
                ItemsElementName = new[] { AuthnContextType.AuthnContextClassRef },
                Items = new[] { AuthnticationContexts.PasswordProtectedTransport }
            };
        }
    }
}