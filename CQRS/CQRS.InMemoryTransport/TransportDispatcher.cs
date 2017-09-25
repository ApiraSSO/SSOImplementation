using System.IO;
using System.Threading.Tasks;
using Kernel.CQRS.Messaging;
using Kernel.CQRS.Transport;
using Kernel.Serialisation;

namespace CQRS.InMemoryTransport
{
    internal class TransportDispatcher : ITranspontDispatcher
    {
        private readonly ISerializer _serialiser;
        private readonly ITransportManager _transport;
        public TransportDispatcher(ITransportManager transport, ISerializer serialiser)
        {
            this._serialiser = serialiser;
            this._transport = transport;
        }
        public Task SendMessage<TMessage>(TMessage message) where TMessage : Message
        {
            using (var ms = new MemoryStream())
            {
                this._serialiser.Serialize(ms, new object[] { message });
                var serialsed = ms.ToArray();
                this._transport.EnqueueMessage(serialsed);

            }
            return Task.CompletedTask;
        }
    }
}