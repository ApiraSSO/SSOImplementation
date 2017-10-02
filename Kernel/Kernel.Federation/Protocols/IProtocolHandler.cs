using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface IProtocolHandler<TBinding> where TBinding : IBindingHandler
    {
        Task HandleRequest(SamlProtocolContext context);
        Task HandleResponse(SamlProtocolContext context);
    }
}