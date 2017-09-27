using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Federation.Protocols.Request;
using Federation.Protocols.Test.Mock;
using Kernel.Federation.Protocols;
using NUnit.Framework;
using SecurityManagement;
using Serialisation.Xml;

namespace Federation.Protocols.Test
{
    [TestFixture]
    public class AuthnRequestTests
    {
        [Test]
        public void SerialiseRequest()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var authnRequestContext = new AuthnRequestContext(requestUri, "local");
            var relyingPartyContextBuilder = new RelyingPartyContextBuilderMock();
            var serialiser = new XMLSerialiser();
            var certManager = new CertificateManager();
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext, relyingPartyContextBuilder);

            //ACT
            var request = AuthnRequestHelper.Serialise(authnRequest, serialiser);
            //var request = AuthnRequestHelper.SerialiseAndSign(authnRequest, authnRequestContext, serialiser, relyingPartyContextBuilder, certManager);
            //ASSERT
        }
    }
}
