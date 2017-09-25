using CQRS.InMemoryTransport;
using NUnit.Framework;

namespace CQRS.InMemoryTransportTests.InMemoryTransportTests
{
    [TestFixture]
    internal class TransportTests
    {
        [Test]
        public void EnqueueMessageTest()
        {
            //ARRANGE
            var transport = new InMemoryQueueTransport();
            var message = new byte[] { 0, 1, 2 };
            byte[] dequeuedMessage;
            //ACT
            transport.Start();
            transport.Enque(message);
            var result = transport.TryDequeue(out dequeuedMessage);
            //ASSERT
            Assert.IsTrue(result);
            Assert.AreEqual(message, dequeuedMessage);
        }
    }
}