using System;
using System.Linq;
using System.Threading.Tasks;
using DeflateCompression;
using Federation.Protocols.Endocing;
using Federation.Protocols.Request;
using Federation.Protocols.Test.Mock;
using Kernel.Federation.Protocols;
using NUnit.Framework;
using Serialisation.Xml;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

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
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();

            //ACT
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);
            var audience = ((AudienceRestriction)authnRequest.Conditions.Items.Single())
                .Audience
                .Single();

            //ASSERT
            Assert.NotNull(authnRequest);
            Assert.AreEqual(requestConfiguration.IsPassive, authnRequest.IsPassive);
            Assert.AreEqual(requestConfiguration.ForceAuthn, authnRequest.ForceAuthn);
            Assert.AreEqual("2.0", authnRequest.Version);
            //issuer
            Assert.AreEqual(requestConfiguration.EntityId, authnRequest.Issuer.Value);
            Assert.AreEqual(NameIdentifierFormats.Entity, authnRequest.Issuer.Format);
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Count, authnRequest.Conditions.Items.Count);
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Single(), audience);
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