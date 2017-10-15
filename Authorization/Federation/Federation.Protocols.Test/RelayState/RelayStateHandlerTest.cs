using System.Collections.Generic;
using System.Threading.Tasks;
using DeflateCompression;
using Federation.Protocols.Endocing;
using Federation.Protocols.RelayState;
using Federation.Protocols.Test.Mock;
using Kernel.Federation.Protocols;
using NUnit.Framework;
using Serialisation.JSON;
using Serialisation.JSON.SettingsProviders;

namespace Federation.Protocols.Test.RelayState
{
    [TestFixture]
    internal class RelayStateHandlerTest
    {
        [Test]
        public async Task HandlerTest()
        {
            //ARRANGE
            var relayState = new Dictionary<string, object> { { "relayState", "Test state" } };
            var form = new Dictionary<string, string>();
            var compressor = new DeflateCompressor();
            var messageEncoder = new MessageEncoding(compressor);
            var jsonSerialiser = new NSJsonSerializer(new DefaultSettingsProvider());
            var logger = new LogProviderMock();
            var serialiser = new RelaystateSerialiser(jsonSerialiser, messageEncoder, logger) as IRelayStateSerialiser;
            var handler = new RelayStateHandler(serialiser);
            //ACT
            var serialised = await serialiser.Serialize(relayState);
            form.Add("RelayState", serialised);
            var deserialised = await handler.GetRelayStateFromFormData(form) as Dictionary<string, object>;
            //ASSERT
            Assert.AreEqual(relayState.Count, deserialised.Count);
            Assert.AreEqual(relayState["relayState"], deserialised["relayState"]);
        }
    }
}
