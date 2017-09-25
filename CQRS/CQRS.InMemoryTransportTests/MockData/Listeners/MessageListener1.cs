using System;
using System.Threading.Tasks;
using Kernel.CQRS.Transport;

namespace CQRS.InMemoryTransportTests.MockData.Listeners
{
    internal class MessageListener1 : IMessageListener
    {
        private readonly Action<byte[]> _action;

        public MessageListener1(Action<byte[]> action)
        {
            this._action = action;
        }
        public Task<bool> AttachTo(ITransportManager transportManager)
        {
            transportManager.RegisterListener(this);
            return Task.FromResult(true);
        }

        public Task ReceiveMessage(byte[] message)
        {
            this._action(message);
            return Task.CompletedTask;
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