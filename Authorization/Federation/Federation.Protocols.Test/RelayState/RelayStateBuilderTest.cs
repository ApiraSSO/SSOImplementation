using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeflateCompression;
using Federation.Protocols.Bindings.HttpRedirect.ClauseBuilders;
using Federation.Protocols.Endocing;
using Federation.Protocols.RelayState;
using Federation.Protocols.Test.Mock;
using Kernel.Federation.Protocols;
using NUnit.Framework;
using Serialisation.JSON;
using Serialisation.JSON.SettingsProviders;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Test.RelayState
{
    [TestFixture]
    internal class RelayStateBuilderTest
    {
        [Test]
        public async Task RelayStateBuilder_test()
        {
            //ARRANGE
            var relayState = new Dictionary<string, object> { { "relayState" ,"Test state" } };
            var compressor = new DeflateCompressor();
            var messageEncoder = new MessageEncoding(compressor);
            var jsonSerialiser = new NSJsonSerializer(new DefaultSettingsProvider());
            var logger = new LogProviderMock();
            var serialiser = new RelaystateSerialiser(jsonSerialiser, messageEncoder, logger) as IRelayStateSerialiser;
            
            var context = new BindingContext(relayState, new Uri("localhost:"));
            var builder = new RelayStateBuilder(serialiser);
            //ACT
            await builder.Build(context);
            var result = context.ClauseBuilder.ToString();
            //ASSERT
            Assert.IsTrue(result.StartsWith(String.Format("&{0}", HttpRedirectBindingConstants.RelayState)));
        }
    }
}
