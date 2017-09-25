using System;
using CQRS.Infrastructure.Messaging;

namespace CQRS.MessageHandling.Test.MockData.MessageHandling
{
    internal class TestCommand : Command
    {
        public TestCommand(Guid id, Guid correlationId) : base(id, correlationId)
        {
        }
    }
}
