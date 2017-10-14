using System;
using System.Collections.Generic;
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

namespace Federation.Protocols.Test.Request
{
    [TestFixture]
    public class AuthnRequestNameIdTests
    {
        [Test]
        public void BuildAuthnRequest_test_nameid_fortmat_match()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local", NameIdentifierFormats.Transient);
            var supportedNameIdentifierFormats = new List<Uri> { new Uri(NameIdentifierFormats.Transient) };
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
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
            //audience
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Count, authnRequest.Conditions.Items.Count);
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Single(), audience);
            //nameIdPolicy
            Assert.IsFalse(authnRequest.NameIdPolicy.AllowCreate);
            Assert.AreEqual(authnRequest.NameIdPolicy.Format, NameIdentifierFormats.Transient);
        }

        [Test]
        public void BuildAuthnRequest_test_nameid_fortmat_match_from_many_entries_supported()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local", NameIdentifierFormats.Persistent);
            var supportedNameIdentifierFormats = new List<Uri> { new Uri(NameIdentifierFormats.Transient), new Uri(NameIdentifierFormats.Persistent) };
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
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
            //audience
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Count, authnRequest.Conditions.Items.Count);
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Single(), audience);
            //nameIdPolicy
            Assert.IsFalse(authnRequest.NameIdPolicy.AllowCreate);
            Assert.AreEqual(authnRequest.NameIdPolicy.Format, NameIdentifierFormats.Persistent);
        }

        [Test]
        public void BuildAuthnRequest_test_nameid_fortmat_no_match_from_many_entries_supported()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local", NameIdentifierFormats.Windows);
            var supportedNameIdentifierFormats = new List<Uri> { new Uri(NameIdentifierFormats.Transient), new Uri(NameIdentifierFormats.Persistent) };
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
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
            //audience
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Count, authnRequest.Conditions.Items.Count);
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Single(), audience);
            //nameIdPolicy
            Assert.IsFalse(authnRequest.NameIdPolicy.AllowCreate);
            Assert.AreEqual(authnRequest.NameIdPolicy.Format, NameIdentifierFormats.Unspecified);
        }

        [Test]
        public void BuildAuthnRequest_test_nameid_fortmat_no_match_none_supported()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local", NameIdentifierFormats.Persistent);
            var supportedNameIdentifierFormats = new List<Uri>();
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
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
            //audience
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Count, authnRequest.Conditions.Items.Count);
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Single(), audience);
            //nameIdPolicy
            Assert.IsFalse(authnRequest.NameIdPolicy.AllowCreate);
            Assert.AreEqual(authnRequest.NameIdPolicy.Format, NameIdentifierFormats.Unspecified);
        }

        [Test]
        public void BuildAuthnRequest_test_nameid_fortmat_encrypted()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local", NameIdentifierFormats.Encrypted);
            var supportedNameIdentifierFormats = new List<Uri> { new Uri(NameIdentifierFormats.Transient), new Uri(NameIdentifierFormats.Persistent) };
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
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
            //audience
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Count, authnRequest.Conditions.Items.Count);
            Assert.AreEqual(requestConfiguration.AudienceRestriction.Single(), audience);
            //nameIdPolicy
            Assert.IsFalse(authnRequest.NameIdPolicy.AllowCreate);
            Assert.AreEqual(authnRequest.NameIdPolicy.Format, NameIdentifierFormats.Encrypted);
        }

        [Test]
        public async Task AuthnRequestSerialiser_test()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local");
            var supportedNameIdentifierFormats = new List<Uri> { new Uri(NameIdentifierFormats.Transient) };
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);

            var xmlSerialiser = new XMLSerialiser();
            var compressor = new DeflateCompressor();
            var encoder = new MessageEncoding(compressor);
            var logger = new LogProviderMock();
            var serialiser = new AuthnRequestSerialiser(xmlSerialiser, encoder, logger);
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);

            //ACT
            var request = await serialiser.Serialize(authnRequest);

            //ASSERT
            Assert.NotNull(request);
        }
    }
}