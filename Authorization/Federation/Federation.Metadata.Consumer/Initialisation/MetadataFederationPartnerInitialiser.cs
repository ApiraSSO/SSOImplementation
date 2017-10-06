using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Federation.Metadata.FederationPartner.Configuration;
using Federation.Metadata.FederationPartner.Handlers;
using Kernel.DependancyResolver;
using Kernel.Federation.MetaData;
using Shared.Initialisation;

namespace Federation.Metadata.FederationPartner.Initialisation
{
    public class MetadataFederationPartnerInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<WsFederationConfigurationRetriever>(Lifetime.Transient);
            dependencyResolver.RegisterType<FederationConfigurationManager>(Lifetime.Singleton);
            dependencyResolver.RegisterType<MetadataEntitiesDescriptorHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<MetadataEntitityDescriptorHandler>(Lifetime.Transient);
            dependencyResolver.RegisterFactory<Action<MetadataBase>>(() => m => 
            {
                this.OnReCeived(m, dependencyResolver);

            }, Lifetime.Singleton);
            return Task.CompletedTask;
        }

        //ToDo Sort this out
        private void OnReCeived(MetadataBase m, IDependencyResolver dependencyResolver)
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