using System;
using System.Threading.Tasks;
using DeflateCompression;
using Federation.Protocols.Endocing;
using Federation.Protocols.Request;
using Federation.Protocols.Test.Mock;
using Kernel.Federation.Protocols;
using NUnit.Framework;
using Serialisation.Xml;

namespace Federation.Protocols.Test
{
    [TestFixture]
    public class AuthnRequestTests
    {
        [Test]
        public void BuildAuthnRequest_test()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local");
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex);
            
            //ACT
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);

            //ASSERT
            Assert.NotNull(authnRequest);
            Assert.AreEqual("2.0", authnRequest.Version);

        }

        [Test]
        public async Task AuthnRequestSerialiser_test()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local");
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex);

            var xmlSerialiser = new XMLSerialiser();
            var compressor = new DeflateCompressor();
            var encoder = new MessageEncoding(compressor);
            var serialiser = new AuthnRequestSerialiser(xmlSerialiser, encoder);
            
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);

            //ACT
            var request = await serialiser.Serialize(authnRequest);

            //ASSERT
            Assert.NotNull(request);
        }
    }
}