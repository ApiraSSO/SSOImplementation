using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;
using Kernel.Initialisation;
using Kernel.Reflection;

namespace Federation.Protocols.Bindings.HttpRedirect
{
    internal class HttpRederectBindingHandler : IBindingHandler<HttpRedirectContext>
    {
        private static Func<Type, bool> _condition = t => !t.IsAbstract && !t.IsInterface && typeof(ISamlClauseBuilder).IsAssignableFrom(t);

        public Task BuildRequest(BindingContext context)
        {
            return this.BuildRequest((HttpRedirectContext)context);
        }

        public async Task BuildRequest(HttpRedirectContext context)
        {
            var builders = this.GetBuilders();
            foreach(var b in builders.OrderBy(x => x.Order))
            {
                await b.Build(context);
            }
        }

        public Task HandleResponse<TResponse>(TResponse response)
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