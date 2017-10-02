using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Bindings.HttpRedirectBinding;
using Kernel.Initialisation;

namespace Federation.Protocols.Bindings.HttpRedirect
{
    internal class HttpRedirectBindingHandler : IBindingHandler
    {
        private static Func<Type, bool> _condition = t => !t.IsAbstract && !t.IsInterface && typeof(ISamlClauseBuilder).IsAssignableFrom(t);
        
        public async Task BuildRequest(HttpRedirectContext context)
        {
            var builders = this.GetBuilders();
            foreach(var b in builders.OrderBy(x => x.Order))
            {
                await b.Build(context);
            }
        }

        public async Task HandleRequest(SamlRequestContext context)
        {
            var builders = this.GetBuilders();
            foreach (var b in builders.OrderBy(x => x.Order))
            {
                await b.Build(context.BindingContext);
            }
            var httpRedirectContext = context as HttpRedirectRequestContext;
            await httpRedirectContext.RequestHanlerAction(context.BindingContext.GetDestinationUrl());
        }

        public Task HandleResponse(SamlResponseContext context)
        {
            throw new NotImplementedException();
        }

        private  IEnumerable<ISamlClauseBuilder> GetBuilders()
        {
            var resolver = ApplicationConfiguration.Instance.DependencyResolver;
            return resolver.ResolveAll<ISamlClauseBuilder>();
        }
    }
}