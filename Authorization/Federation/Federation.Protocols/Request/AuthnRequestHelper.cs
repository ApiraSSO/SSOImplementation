using System;
using System.Collections.Generic;
using System.Linq;
using Federation.Protocols.Request.ClauseBuilders;
using Kernel.Federation.Protocols;
using Kernel.Reflection;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request
{
    internal class AuthnRequestHelper
    {
        private static Func<Type, bool> _condition = t => !t.IsAbstract && !t.IsInterface && typeof(IAuthnRequestClauseBuilder<AuthnRequest>).IsAssignableFrom(t);
        internal static AuthnRequest BuildAuthnRequest(AuthnRequestContext authnRequestContext)
        {
            var requestConfig = authnRequestContext.FederationPartyContext.GetRequestConfigurationFromContext();

            var request = new AuthnRequest
            {
                IsPassive = requestConfig.IsPassive,
                ForceAuthn = requestConfig.ForceAuthn,
                Destination = authnRequestContext.Destination.AbsoluteUri,
                Version = requestConfig.Version,
                IssueInstant = DateTime.UtcNow
            };
            
            var buiders = AuthnRequestHelper.GetBuilders();
            foreach(var b in buiders)
            {
                b.Build(request, requestConfig);
            }
            return request;
        }

        private static IEnumerable<IAuthnRequestClauseBuilder<AuthnRequest>> GetBuilders()
        {
            return ReflectionHelper.GetAllTypes(new[] { typeof(ClauseBuilder).Assembly }, t => AuthnRequestHelper._condition(t))
                .Select(x => (IAuthnRequestClauseBuilder<AuthnRequest>)Activator.CreateInstance(x));
        }
    }
}