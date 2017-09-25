using System.Threading.Tasks;
using Kernel.CQRS.Messaging;

namespace Kernel.CQRS.Transport
{
    public interface ITransportManager
    {
        Task Initialise();
        Task Start();
        Task Stop();
        Task<bool> EnqueueMessage(byte[] message);
        Task RegisterListener(IMessageListener listener);
    }
}