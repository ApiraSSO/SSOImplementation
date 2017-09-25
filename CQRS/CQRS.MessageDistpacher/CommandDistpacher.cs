using System;
using System.Threading.Tasks;
using CQRS.Infrastructure.Messaging;
using Kernel.CQRS.Dispatching;
using Kernel.CQRS.Messaging;
using Kernel.CQRS.Transport;

namespace CQRS.MessageDistpacher
{
    public class CommandDistpacher : IMessageDispatcher<Command>
    {
        private readonly ITranspontDispatcher _transpontHandler;

        public CommandDistpacher(ITranspontDispatcher transpontHandler)
        {
            this._transpontHandler = transpontHandler;
        }
        Task IMessageDispatcher.SendMessage(Message message)
        {
            return Task.Factory.StartNew(async () =>
            {
                try
                {
                    await _transpontHandler.SendMessage(message);
                }
                catch (Exception e)
                {
                    HandleError(message, e);
                }
            });
        }

        public Task SendMessage(Command message)
        {
            return ((IMessageDispatcher)this).SendMessage(message);
        }

        private void HandleError(Message message, Exception e)
        {
            throw new NotImplementedException();
        }
    }
}