using System;
using System.Collections.Generic;
using Federation.Protocols.Request;
using Federation.Protocols.Test.Mock;
using Kernel.Federation.Protocols;
using NUnit.Framework;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Test.Request
{
    [TestFixture]
    internal class AuthnRequestRequestedContextTests
    {
        [Test]
        public void BuildAuthnRequest_test_requested_authn_context_default()
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

            //ASSERT
            Assert.NotNull(authnRequest);
            Assert.IsNotNull(authnRequest.RequestedAuthnContext);
            Assert.AreEqual(AuthnContextComparisonType.Exact, authnRequest.RequestedAuthnContext.Comparison);
            Assert.AreEqual(1, authnRequest.RequestedAuthnContext.Items.Length);
            Assert.AreEqual(1, authnRequest.RequestedAuthnContext.ItemsElementName.Length);
            Assert.AreEqual(AuthnContextType.AuthnContextClassRef, authnRequest.RequestedAuthnContext.ItemsElementName[0]);
            Assert.AreEqual("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport", authnRequest.RequestedAuthnContext.Items[0]);
        }

        [Test]
        public void BuildAuthnRequest_test_requested_authn_context_default_overwritten()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local", NameIdentifierFormats.Transient);
            federationContex.RequestedAuthnContextConfiguration = new Kernel.Federation.FederationPartner.RequestedAuthnContextConfiguration(AuthnContextComparisonType.Minimum.ToString());
            federationContex.RequestedAuthnContextConfiguration.RequestedAuthnContexts.Add(new Kernel.Federation.Protocols.AuthnContext(AuthnContextType.AuthnContextClassRef.ToString(), new Uri(AuthnticationContexts.Password)));
            var supportedNameIdentifierFormats = new List<Uri> { new Uri(NameIdentifierFormats.Transient) };
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
            //ACT
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);

            //ASSERT
            Assert.NotNull(authnRequest);
            Assert.IsNotNull(authnRequest.RequestedAuthnContext);
            Assert.AreEqual(AuthnContextComparisonType.Minimum, authnRequest.RequestedAuthnContext.Comparison);
            Assert.AreEqual(1, authnRequest.RequestedAuthnContext.Items.Length);
            Assert.AreEqual(1, authnRequest.RequestedAuthnContext.ItemsElementName.Length);
            Assert.AreEqual(AuthnContextType.AuthnContextClassRef, authnRequest.RequestedAuthnContext.ItemsElementName[0]);
            Assert.AreEqual(AuthnticationContexts.Password, authnRequest.RequestedAuthnContext.Items[0]);
        }

        [Test]
        public void BuildAuthnRequest_test_requested_authn_context_default_overwritten_multiple_contexts()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local", NameIdentifierFormats.Transient);
            federationContex.RequestedAuthnContextConfiguration = new Kernel.Federation.FederationPartner.RequestedAuthnContextConfiguration(AuthnContextComparisonType.Minimum.ToString());
            federationContex.RequestedAuthnContextConfiguration.RequestedAuthnContexts.Add(new Kernel.Federation.Protocols.AuthnContext(AuthnContextType.AuthnContextClassRef.ToString(), new Uri(AuthnticationContexts.Password)));
            federationContex.RequestedAuthnContextConfiguration.RequestedAuthnContexts.Add(new Kernel.Federation.Protocols.AuthnContext(AuthnContextType.AuthnContextClassRef.ToString(), new Uri(AuthnticationContexts.PasswordProtectedTransport)));

            var supportedNameIdentifierFormats = new List<Uri> { new Uri(NameIdentifierFormats.Transient) };
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex, supportedNameIdentifierFormats);
            var requestConfiguration = federationContex.GetRequestConfigurationFromContext();
            AuthnRequestHelper.GetBuilders = AuthnRequestBuildersFactoryMock.GetBuildersFactory();
            //ACT
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);

            //ASSERT
            Assert.NotNull(authnRequest);
            Assert.IsNotNull(authnRequest.RequestedAuthnContext);
            Assert.AreEqual(AuthnContextComparisonType.Minimum, authnRequest.RequestedAuthnContext.Comparison);
            Assert.AreEqual(2, authnRequest.RequestedAuthnContext.Items.Length);
            Assert.AreEqual(2, authnRequest.RequestedAuthnContext.ItemsElementName.Length);
            Assert.AreEqual(AuthnContextType.AuthnContextClassRef, authnRequest.RequestedAuthnContext.ItemsElementName[0]);
            Assert.AreEqual(AuthnticationContexts.Password, authnRequest.RequestedAuthnContext.Items[0]);
            Assert.AreEqual(AuthnContextType.AuthnContextClassRef, authnRequest.RequestedAuthnContext.ItemsElementName[1]);
            Assert.AreEqual(AuthnticationContexts.PasswordProtectedTransport, authnRequest.RequestedAuthnContext.Items[1]);
        }
    }
}