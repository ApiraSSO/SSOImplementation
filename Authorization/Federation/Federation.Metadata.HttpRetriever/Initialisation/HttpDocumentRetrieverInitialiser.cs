using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace Federation.Metadata.HttpRetriever.Initialisation
{
    public class HttpDocumentRetrieverInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<HttpDocumentRetriever>(Lifetime.Transient);
            return Task.CompletedTask;
        }
    }
}