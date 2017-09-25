using System.Threading.Tasks;
using Federation.Metadata.RelyingParty.Configuration;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace Federation.Metadata.RelyingParty.Initialisation
{
    public class MetadataConsumerInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<WsFederationConfigurationRetriever>(Lifetime.Transient);
            return Task.CompletedTask;
        }
    }
}