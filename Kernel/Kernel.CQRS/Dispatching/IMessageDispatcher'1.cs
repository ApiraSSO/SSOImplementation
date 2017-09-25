using System.Threading.Tasks;
using Kernel.CQRS.Messaging;

namespace Kernel.CQRS.Dispatching
{
    public interface IMessageDispatcher<TMessage> : IMessageDispatcher where TMessage : Message
    {
        Task SendMessage(TMessage message);
    }
}