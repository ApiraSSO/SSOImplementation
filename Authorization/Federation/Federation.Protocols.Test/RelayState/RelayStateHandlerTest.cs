using System.Collections.Generic;
using System.Threading.Tasks;
using DeflateCompression;
using Federation.Protocols.Endocing;
using Federation.Protocols.RelayState;
using Kernel.Federation.Protocols;
using NUnit.Framework;

namespace Federation.Protocols.Test.RelayState
{
    [TestFixture]
    internal class RelayStateHandlerTest
    {
        [Test]
        public async Task HandlerTest()
        {
            //ARRANGE
            var relayState = "Test state";
            var form = new Dictionary<string, string>();
            var compressor = new DeflateCompressor();
            var messageEncoder = new MessageEncoding(compressor);
            var serialiser = new RelaystateSerialiser(messageEncoder) as IRelayStateSerialiser;
            var handler = new RelayStateHandler(serialiser);
            //ACT
            var serialised = await serialiser.Serialize(relayState);
            form.Add("RelayState", serialised);
            var deserialised = await handler.GetRelayStateFromFormData(form);
            //ASSERT
            Assert.AreEqual(relayState, deserialised);
        }
    }
}
