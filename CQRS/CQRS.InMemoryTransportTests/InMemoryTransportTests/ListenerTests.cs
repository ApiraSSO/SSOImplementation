using System.Threading;
using CQRS.InMemoryTransport;
using CQRS.InMemoryTransportTests.MockData.Listeners;
using NUnit.Framework;

namespace CQRS.InMemoryTransportTests.InMemoryTransportTests
{
    [TestFixture]
    internal class ListenerTests
    {
        [Test]
        public void ListnerTest_one_listener()
        {
            //ARRANGE
            var transport = new InMemoryQueueTransport();
            var manager = new TransportManager(transport);
            var message = new byte[] { 0, 1, 2 };
            
            byte[] messageReceived = null;
            
            var listener = new MessageListener1(m => messageReceived = m);
            //ACT
            listener.AttachTo(manager);
            transport.Start();
            manager.EnqueueMessage(message);
            Thread.Sleep(500);
            //ASSERT
            
            Assert.AreEqual(message, messageReceived);
        }

        [Test]
        public void ListnerTest_2_listeners()
        {
            //ARRANGE
            var transport = new InMemoryQueueTransport();
            var manager = new TransportManager(transport);
            var message = new byte[] { 0, 1, 2 };

            byte[] messageReceived1 = null;
            byte[] messageReceived2 = null;

            var listener1 = new MessageListener1(m => messageReceived1= m);
            var listener2 = new MessageListener2(m => messageReceived2 = m);
            //ACT
            listener1.AttachTo(manager);
            listener2.AttachTo(manager);
            transport.Start();
            manager.EnqueueMessage(message);
            Thread.Sleep(500);
            //ASSERT

            Assert.AreEqual(message, messageReceived1);
            Assert.AreEqual(message, messageReceived2);
        }
    }
}