using System;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Federation.Metadata.FederationPartner.Handlers;
using Kernel.DependancyResolver;
using Kernel.Federation.MetaData;
using Shared.Federtion.Factories;

namespace Federation.Metadata.FederationPartner.Configuration
{
    internal class ConfigurationHelper
    {
        //ToDo Sort this out propertly
        public static void OnReceived(MetadataBase metadata, IDependencyResolver dependencyResolver)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            if (dependencyResolver == null)
                throw new ArgumentNullException("dependencyResolver");
            
            string entityId = "RegisteredIssuer";
            var handlerType = typeof(IMetadataHandler<>).MakeGenericType(metadata.GetType());
            var handler = dependencyResolver.Resolve(handlerType);

            var del = IdpMetadataHandlerFactory.GetDelegateForIdpDescriptors(metadata.GetType(), typeof(IdentityProviderSingleSignOnDescriptor));
            var idps = del(handler, metadata).Cast<IdentityProviderSingleSignOnDescriptor>();

            var identityRegister = SecurityTokenHandlerConfiguration.DefaultIssuerNameRegistry as ConfigurationBasedIssuerNameRegistry;
            if (identityRegister == null)
                throw new NotSupportedException();
            var foo = idps.SelectMany(x => x.Keys.SelectMany(y => y.KeyInfo.Select(cl =>
            {
                var bi = cl as BinaryKeyIdentifierClause;
                var raw = bi.GetBuffer();
                var cert = new X509Certificate2(raw);
                return cert;
            }))).Aggregate(identityRegister, (t, next) => 
            {
                if (!identityRegister.ConfiguredTrustedIssuers.Keys.Contains(next.Thumbprint))
                    identityRegister.AddTrustedIssuer(next.Thumbprint, entityId);
                return t;
            });
        }
    }
}