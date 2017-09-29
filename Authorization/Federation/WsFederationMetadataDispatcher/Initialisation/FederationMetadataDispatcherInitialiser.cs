using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace WsFederationMetadataDispatcher.Initialisation
{
    public class FederationMetadataDispatcherInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<FederationMetadataDispatcher>(Lifetime.Transient);

            return Task.CompletedTask;
        }
    }
}