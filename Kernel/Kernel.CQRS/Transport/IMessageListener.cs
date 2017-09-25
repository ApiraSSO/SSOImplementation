using System.Threading.Tasks;

namespace Kernel.CQRS.Transport
{
    public interface IMessageListener
    {
        Task<bool> Start();
        Task<bool> Stop();
        Task<bool> AttachTo(ITransportManager transportManager);
        Task ReceiveMessage(byte[] message);
    }
}