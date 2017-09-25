using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace InlineMetadataContextProvider.Initialisation
{
    public class InlineMetadaContextProviderInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<InlineMetadataContextBuilder>(Lifetime.Transient);
            return Task.CompletedTask;
        }
    }
}