using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace DragonCMS.KafkaClient.Initialisation
{
    public class KafkaClientInitialiser : Initialiser
    {
        public override byte Order
        {
            get
            {
                return 0;
            }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            return Task.CompletedTask;
        }
    }
}