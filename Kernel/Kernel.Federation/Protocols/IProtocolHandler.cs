using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface IProtocolHandler
    {
        Task HandleRequest(SamlProtocolContext context);
        Task HandleResponse(SamlProtocolContext context);
    }
    public interface IProtocolHandler<TBinding> : IProtocolHandler where TBinding : IBindingHandler
    {
        
    }
}