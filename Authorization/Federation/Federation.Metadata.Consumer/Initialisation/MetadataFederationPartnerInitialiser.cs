using System;
using System.IdentityModel.Metadata;
using System.Threading.Tasks;
using Federation.Metadata.FederationPartner.Configuration;
using Federation.Metadata.FederationPartner.Handlers;
using Kernel.DependancyResolver;
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
                ConfigurationHelper.OnReceived(m, dependencyResolver);

            }, Lifetime.Singleton);
            return Task.CompletedTask;
        }
    }
}