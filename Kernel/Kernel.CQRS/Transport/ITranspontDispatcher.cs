using System.Threading.Tasks;
using Kernel.CQRS.Messaging;

namespace Kernel.CQRS.Transport
{
    public interface ITranspontDispatcher
    {
        Task SendMessage<TMessage>(TMessage message) where TMessage : Message;
    }
}
