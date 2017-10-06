using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Federation.Metadata.FederationPartner.Handlers;
using Kernel.DependancyResolver;
using Kernel.Federation.MetaData;

namespace Federation.Metadata.FederationPartner.Configuration
{
    internal class ConfigurationHelper
    {
        //ToDo Sort this out propertly
        public static void OnReceived(MetadataBase m, IDependencyResolver dependencyResolver)
        {
            IEnumerable<IdentityProviderSingleSignOnDescriptor> idps = Enumerable.Empty<IdentityProviderSingleSignOnDescriptor>();
            string entityId = "RegisteredIssuer";
            var handlerType = typeof(IMetadataHandler<>).MakeGenericType(m.GetType());
            var handler = dependencyResolver.Resolve(handlerType);

            var del = HandlerFactory.GetDelegateForIdpDescriptors(m.GetType(), typeof(IdentityProviderSingleSignOnDescriptor));
            idps = del(handler, m).Cast<IdentityProviderSingleSignOnDescriptor>();

            var identityRegister = SecurityTokenHandlerConfiguration.DefaultIssuerNameRegistry as ConfigurationBasedIssuerNameRegistry;
            if (identityRegister == null)
                throw new NotSupportedException();

            foreach (var d in idps)
            {
                foreach (var k in d.Keys)
                {
                    var kinfo = k.KeyInfo;
                    foreach (var c in kinfo)
                    {
                        var bi = c as BinaryKeyIdentifierClause;

                        var raw = bi.GetBuffer();
                        var cert1 = new X509Certificate2(raw);
                        if (identityRegister.ConfiguredTrustedIssuers.Keys.Contains(cert1.Thumbprint))
                            continue;
                        identityRegister.AddTrustedIssuer(cert1.Thumbprint, entityId);
                    }
                }
            }
        }
    }
}
