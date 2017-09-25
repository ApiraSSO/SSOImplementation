using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace OAuthAuthorisationService.Initialisation
{
    public class OAuthAuthorisationServiceInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<OAuthAuthorizationServerOptionsProvider>(Lifetime.Transient);
            dependencyResolver.RegisterType<UserOAuthProvider>(Lifetime.Transient);
            
            return Task.CompletedTask;
        }
    }
}