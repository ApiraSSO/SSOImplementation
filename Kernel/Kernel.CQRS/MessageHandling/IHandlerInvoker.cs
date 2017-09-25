using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.CQRS.MessageHandling
{
    public interface IHandlerInvoker
    {
        Task InvokeHandlers(IEnumerable<object> handlers, object message);
    }
}