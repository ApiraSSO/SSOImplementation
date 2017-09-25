using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace MemoryCacheProvider.Initialisation
{
    public class CacheProviderInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<MemoryCacheRuntimeImplementor>(Lifetime.Singleton);
            return Task.CompletedTask;
        }
    }
}