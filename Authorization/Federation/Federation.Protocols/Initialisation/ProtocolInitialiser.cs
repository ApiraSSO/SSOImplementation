using System;
using System.Threading.Tasks;
using Federation.Protocols.Bindings.HttpPost;
using Federation.Protocols.Bindings.HttpRedirect;
using Federation.Protocols.Bindings.HttpRedirect.ClauseBuilders;
using Federation.Protocols.Endocing;
using Federation.Protocols.RelayState;
using Federation.Protocols.Response;
using Federation.Protocols.Tokens;
using Federation.Protocols.Tokens.Validation;
using Kernel.DependancyResolver;
using Kernel.Federation.Protocols;
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
            dependencyResolver.RegisterType<ResponseHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<SecurityTokenHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<TokenHandlerConfigurationProvider>(Lifetime.Transient);
            dependencyResolver.RegisterType<UserClaimsProvider>(Lifetime.Transient);
            dependencyResolver.RegisterType<MessageEncoding>(Lifetime.Transient);
            dependencyResolver.RegisterType<HttpRedirectBindingHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<SamlRequestBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterType<RelayStateBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterType<SignatureBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterType<RelayStateHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<RelaystateSerialiser>(Lifetime.Transient);
            dependencyResolver.RegisterType<SubjectConfirmationDataValidator>(Lifetime.Transient);
            dependencyResolver.RegisterType<TokenSerialiser>(Lifetime.Transient);

            dependencyResolver.RegisterFactory<Func<Type, object>>(() => dependencyResolver.Resolve, Lifetime.Transient);
            dependencyResolver.RegisterFactory< Func<string, IProtocolHandler> >(() =>
            {
                return b =>
                {
                    if (b == Kernel.Federation.MetaData.Configuration.Bindings.Http_Redirect)
                    {
                        return new ProtocolHandler<HttpRedirectBindingHandler>(new HttpRedirectBindingHandler());
                    }
                    if (b == Kernel.Federation.MetaData.Configuration.Bindings.Http_Post)
                    {
                        var bh = dependencyResolver.Resolve<HttpPostBindingHandler>();
                        return new ProtocolHandler<HttpPostBindingHandler>(bh);
                    }
                    throw new NotSupportedException();

                };
            }, Lifetime.Singleton);

            return Task.CompletedTask;
        }
    }
}