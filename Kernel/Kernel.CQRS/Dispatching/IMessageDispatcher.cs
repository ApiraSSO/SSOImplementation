using System.Threading.Tasks;
using Kernel.CQRS.Messaging;
using Kernel.Initialisation;

namespace Kernel.CQRS.Dispatching
{
    public interface IMessageDispatcher : IAutoRegisterAsTransient
    {
        Task SendMessage(Message message);
    }
}