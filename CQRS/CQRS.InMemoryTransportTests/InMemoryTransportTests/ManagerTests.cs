using CQRS.InMemoryTransport;
using NUnit.Framework;

namespace CQRS.InMemoryTransportTests.InMemoryTransportTests
{
    [TestFixture]
    internal class ManagerTests
    {
        [Test]
        public void EnqueueMessageTest()
        {
            //ARRANGE
            var transport = new InMemoryQueueTransport();
            var manager = new TransportManager(transport);
            var message = new byte[] { 0, 1, 2 };
            byte[] dequeuedMessage;
            //ACT
            transport.Start();
            manager.EnqueueMessage(message);
            var result = transport.TryDequeue(out dequeuedMessage);
            //ASSERT
            Assert.IsTrue(result);
            Assert.AreEqual(message, dequeuedMessage);
        }
    }
}