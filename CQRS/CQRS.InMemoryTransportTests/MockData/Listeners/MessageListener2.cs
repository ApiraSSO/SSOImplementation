using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.CQRS.Transport;

namespace CQRS.InMemoryTransportTests.MockData.Listeners
{
    internal class MessageListener2 : IMessageListener
    {
        private readonly Action<byte[]> _action;

        public MessageListener2(Action<byte[]> action)
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