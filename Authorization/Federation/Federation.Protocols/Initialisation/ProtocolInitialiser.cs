﻿using System;
using System.Threading.Tasks;
using Federation.Protocols.Bindings.HttpRedirect;
using Federation.Protocols.Bindings.HttpRedirect.ClauseBuilders;
using Federation.Protocols.Endocing;
using Federation.Protocols.Request;
using Federation.Protocols.Response;
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
            dependencyResolver.RegisterType<AuthnRequestBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterType<ResponseHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<Saml2SecurityTokenHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<TokenHandlerConfigurationProvider>(Lifetime.Transient);
            dependencyResolver.RegisterType<UserClaimsProvider>(Lifetime.Transient);
            dependencyResolver.RegisterType<MessageEncoding>(Lifetime.Transient);
            dependencyResolver.RegisterType<HttpRedirectBindingHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<SamlRequestBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterType<RelayStateBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterType<SignatureBuilder>(Lifetime.Transient);
            dependencyResolver.RegisterFactory< Func<string, IProtocolHandler> >(() =>
            {
                return b =>
                {
                    if (b == Kernel.Federation.MetaData.Configuration.Bindings.Http_Redirect)
                    {
                        return new ProtocolHandler<HttpRedirectBindingHandler>(new HttpRedirectBindingHandler());
                    }
                    throw new NotSupportedException();
                };
            }, Lifetime.Singleton);

            return Task.CompletedTask;
        }
    }
}