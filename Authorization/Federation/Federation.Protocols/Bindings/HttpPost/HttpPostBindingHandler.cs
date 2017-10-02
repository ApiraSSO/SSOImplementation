using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Bindings.HttpPost
{
    internal class HttpPostBindingHandler : IBindingHandler<HttpPostContext>
    {
        private readonly IReponseHandler<Func<string, Task<ClaimsIdentity>>> _responseHandler;

        public HttpPostBindingHandler(IReponseHandler<Func<string, Task<ClaimsIdentity>>> responseHandler)
        {
            this._responseHandler = responseHandler;
        }

        public Task BuildRequest(BindingContext context)
        {
            return this.BuildRequest((HttpPostContext)context);
        }

        public Task BuildRequest(HttpPostContext context)
        {
            throw new NotImplementedException();
        }

        public Task HandleResponse<TResponse>(TResponse response)
        {
            throw new NotImplementedException();
        }
    }
}