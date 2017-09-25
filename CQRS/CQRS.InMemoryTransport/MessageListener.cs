using System;
using System.Threading.Tasks;
using Kernel.CQRS.Transport;

namespace CQRS.InMemoryTransport
{
    internal class MessageListener : IMessageListener
    {
        public Task<bool> AttachTo(ITransportManager transportManager)
        {
            transportManager.RegisterListener(this);
            return Task.FromResult(true);
        }

        public Task ReceiveMessage(byte[] message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Start()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Stop()
        {
            throw new NotImplementedException();
        }
    }
}