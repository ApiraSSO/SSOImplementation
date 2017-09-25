using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace WebClientMetadataWriter.Initialisation
{
    public class WebClientMetadataWriterInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<SSOMetadataFileWriter>(Lifetime.Transient);
            
            return Task.CompletedTask;
        }
    }
}