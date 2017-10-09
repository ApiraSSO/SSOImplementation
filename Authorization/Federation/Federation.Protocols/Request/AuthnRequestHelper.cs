using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using Federation.Protocols.Request.ClauseBuilders;
using Kernel.Federation.Protocols;
using Kernel.Reflection;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request
{
    internal class AuthnRequestHelper
    {
        private static Func<Type, bool> _condition = t => !t.IsAbstract && !t.IsInterface && typeof(IAuthnRequestClauseBuilder<AuthnRequest>).IsAssignableFrom(t);
        internal static AuthnRequest BuildAuthnRequest(AuthnRequestContext authnRequestContext)
        {
            var request = new AuthnRequest
            {
                IsPassive = false,
                ForceAuthn = false,
                Destination = authnRequestContext.Destination.AbsoluteUri,
                Version = authnRequestContext.Version,
                IssueInstant = DateTime.UtcNow
            };
            var requestConfig = authnRequestContext.FederationPartyContext.GetRequestConfigurationFromContext();
            var buiders = AuthnRequestHelper.GetBuilders();
            foreach(var b in buiders)
            {
                b.Build(request, requestConfig);
            }
            return request;
        }

        internal static void AppendSignarureAlgorithm(StringBuilder builder)
        {
            builder.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.SigAlg, Uri.EscapeDataString(SignedXml.XmlDsigRSASHA1Url));
        }
        
        private static IEnumerable<IAuthnRequestClauseBuilder<AuthnRequest>> GetBuilders()
        {
            return ReflectionHelper.GetAllTypes(new[] { typeof(ClauseBuilder).Assembly }, t => AuthnRequestHelper._condition(t))
                .Select(x => (IAuthnRequestClauseBuilder<AuthnRequest>)Activator.CreateInstance(x));
        }
    }
}