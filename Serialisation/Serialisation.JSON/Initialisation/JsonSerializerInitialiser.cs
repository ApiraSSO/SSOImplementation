using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Serialisation.JSON.SettingsProviders;
using Shared.Initialisation;

namespace Serialisation.JSON.Initialisation
{
    public class JsonSerializerInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<NSJsonSerializer>(Lifetime.Transient);
            dependencyResolver.RegisterType<DefaultSettingsProvider>(Lifetime.Transient);
            return Task.CompletedTask;
        }
    }
}
