using System;
using System.Threading.Tasks;
using Federation.Protocols.Bindings.HttpPost;
using Kernel.Federation.Protocols;

namespace Federation.Protocols
{
    internal class ProtocolHandler<TBinding> : IProtocolHandler<TBinding> where TBinding : IBindingHandler
    {
        private readonly TBinding _bindingHandler;
        

        public ProtocolHandler(TBinding binding)
        {
            this._bindingHandler = binding;
        }
        
        public async Task HandleRequest(SamlProtocolContext context)
        {
            await this._bindingHandler.BuildRequest(context.BindingContext);
            await context.RequestHanlerAction(context.BindingContext.GetDestinationUrl());
        }

        public async Task HandleResponse(SamlProtocolContext context)
        {
            var httpPostResponseContext = context.HttpPostResponseContext as HttpPostResponseContext;
            await this._bindingHandler.HandleResponse(httpPostResponseContext);
        }
    }
}