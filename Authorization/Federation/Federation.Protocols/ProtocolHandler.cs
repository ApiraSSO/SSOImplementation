using System.Threading.Tasks;
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
            await this._bindingHandler.HandleRequest(context.RequestContext);
        }

        public async Task HandleResponse(SamlProtocolContext context)
        {
            await this._bindingHandler.HandleResponse(context.ResponseContext);
        }
    }
}