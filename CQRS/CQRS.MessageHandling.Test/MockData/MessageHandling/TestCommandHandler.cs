using System;
using System.Threading.Tasks;
using Kernel.CQRS.MessageHandling;

namespace CQRS.MessageHandling.Test.MockData.MessageHandling
{
    internal class TestCommandHandler : IMessageHandler<TestCommand>
    {
        private readonly Action _action;
        public  TestCommandHandler(Action action)
        {
            this._action = action;
        }

        public Task Handle(TestCommand command)
        {
            this._action();
            return Task.CompletedTask;
        }
    }
}