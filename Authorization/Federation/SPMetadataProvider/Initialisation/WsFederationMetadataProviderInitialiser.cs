using System;
using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData;
using Shared.Initialisation;
using WsFederationMetadataProvider.Metadata;

namespace WsFederationMetadataProvider.Initialisation
{
    public class WsFederationMetadataProviderInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<SPSSOMetadataProvider>(Lifetime.Transient);
            dependencyResolver.RegisterFactory<Func<MetadataGenerateRequest, FederationPartyConfiguration>>(() => c =>
            {
                var builder = dependencyResolver.Resolve<IFederationPartyContextBuilder>();
                using (builder)
                {
                    return builder.BuildContext(c.FederationPartyId);
                }
            } , Lifetime.Singleton);
           
            return Task.CompletedTask;
        }
    }
}