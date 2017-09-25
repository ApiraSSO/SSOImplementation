using System;
using CQRS.Infrastructure.Messaging;

namespace CQRS.MessageHandling.Test.MockData.MessageHandling
{
    internal class TestEvent : Event
    {
        public TestEvent(Guid id, Guid correlationId) : base(id, correlationId)
        {
        }
    }
}
