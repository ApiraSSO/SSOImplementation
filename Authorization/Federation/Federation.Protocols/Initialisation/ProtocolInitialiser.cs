using System.Threading.Tasks;
using Federation.Protocols.Request;
using Federation.Protocols.Response;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace Federation.Protocols.Initialisation
{
    public class ProtocolInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<AuthnRequestBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterType<ResponseHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<Saml2SecurityTokenHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<TokenHandlerConfigurationProvider>(Lifetime.Transient);

            return Task.CompletedTask;
        }
    }
}