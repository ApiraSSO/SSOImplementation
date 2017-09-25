using System.Threading.Tasks;

namespace Kernel.CQRS.Transport
{
    public interface ITransport
    {
        ITransportManager Manager { get; }
        Task Initialise();
        Task Start();
        Task Stop();
    }
}
