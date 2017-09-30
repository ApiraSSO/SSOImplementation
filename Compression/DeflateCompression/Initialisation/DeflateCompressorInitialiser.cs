using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace DeflateCompression.Initialisation
{
    public class DeflateCompressorInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<DeflateCompressor>(Lifetime.Transient);
           
            return Task.CompletedTask;
        }
    }
}