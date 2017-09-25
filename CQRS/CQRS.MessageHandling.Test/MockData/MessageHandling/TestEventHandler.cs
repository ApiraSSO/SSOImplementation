using System;
using System.Threading.Tasks;
using Kernel.CQRS.MessageHandling;

namespace CQRS.MessageHandling.Test.MockData.MessageHandling
{
    internal class TestEventHandler : IMessageHandler<TestEvent>
    {
        private readonly Action _action;
        public TestEventHandler(Action action)
        {
            this._action = action;
        }

        public Task Handle(TestEvent command)
        {
            this._action();
            return Task.CompletedTask;
        }
    }
}