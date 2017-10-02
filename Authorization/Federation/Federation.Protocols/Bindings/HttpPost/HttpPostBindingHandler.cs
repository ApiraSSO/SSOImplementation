using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Bindings.HttpPostBinding;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Bindings.HttpPost
{
    internal class HttpPostBindingHandler : IBindingHandler
    {
        private readonly IReponseHandler<ClaimsIdentity> _responseHandler;

        public HttpPostBindingHandler(IReponseHandler<ClaimsIdentity> responseHandler)
        {
            this._responseHandler = responseHandler;
        }
        
        public Task HandleRequest(SamlRequestContext context)
        {
            throw new NotImplementedException();
        }
        
        public async Task HandleResponse(SamlResponseContext context)
        {
            var httpPostContext = context as HttpPostResponseContext;
            var result = await this._responseHandler.Handle(httpPostContext);
            httpPostContext.Result = result;
        }
    }
}