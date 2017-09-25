using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Serialisation.CSV.SettingsProviders;
using Shared.Initialisation;

namespace Serialisation.CSV.Initialisation
{
    public class CSVSerializerInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<CSVSerializer>(Lifetime.Transient);
            dependencyResolver.RegisterType<DefaultSettingsProvider>(Lifetime.Transient);
            return Task.CompletedTask;
        }
    }
}
