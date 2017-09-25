using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;
using WsMetadataSerialisation.Serialisation;

namespace WsMetadataSerialisation.Initialisation
{
    public class MetadataSerialisationInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<FederationMetadataSerialiser>(Lifetime.Transient);
            
            return Task.CompletedTask;
        }
    }
}