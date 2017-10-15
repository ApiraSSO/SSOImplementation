using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace Federation.Logging.Initialisation
{
    public class WindowsEventLogLoggerInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<WindowsEventLogLogger>(Lifetime.Transient);
            dependencyResolver.RegisterType<WindowsEventLogWriter>(Lifetime.Transient);
            
            return Task.CompletedTask;
        }
    }
}