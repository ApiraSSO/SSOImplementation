using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Federation.Protocols;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request
{
    internal class AuthnRequestHelper
    {
        internal static Func<IEnumerable<IAuthnRequestClauseBuilder<AuthnRequest>>> GetBuilders { get; set; }

        internal static Func<Type, bool> Condition = t => !t.IsAbstract && !t.IsInterface && typeof(IAuthnRequestClauseBuilder<AuthnRequest>).IsAssignableFrom(t);
        internal static AuthnRequest BuildAuthnRequest(AuthnRequestContext authnRequestContext)
        {
            if (AuthnRequestHelper.GetBuilders == null)
                throw new InvalidOperationException("GetBuilders factory not set");

            var requestConfig = authnRequestContext.FederationPartyContext.GetRequestConfigurationFromContext();

            var request = new AuthnRequest
            {
                IsPassive = requestConfig.IsPassive,
                ForceAuthn = requestConfig.ForceAuthn,
                Destination = authnRequestContext.Destination.AbsoluteUri,
                Version = requestConfig.Version,
                IssueInstant = DateTime.UtcNow
            };
            if(authnRequestContext.SupportedNameIdentifierFormats != null)
            {
                authnRequestContext.SupportedNameIdentifierFormats.Aggregate(requestConfig.SupportedNameIdentifierFormats, (t, next) => 
                {
                    t.Add(next);
                    return t;
                });
            }

            var buiders = AuthnRequestHelper.GetBuilders();
            foreach(var b in buiders)
            {
                b.Build(request, requestConfig);
            }
            return request;
        }
    }
}