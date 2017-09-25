using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Microsoft.AspNet.Identity.Owin.Provider.Factories;
using Microsoft.Owin.Security.DataProtection;
using Shared.Initialisation;

namespace Microsoft.AspNet.Identity.Owin.Provider.Initialisation
{
    public class OwinIdentityInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterFactory(typeof(IUserTokenProvider<,>), t =>
            {
                var genParam = t.GetGenericArguments();
                var dataProtector = new DpapiDataProtectionProvider().Create("OwinIdentity");
                var del = UserTokenProviderFactory.GetTokenProviderDelegate(genParam[0]);
                var tokenProvider = del(dataProtector);
                return tokenProvider;
            }, Lifetime.Transient);

            return Task.CompletedTask;
        }
    }
}