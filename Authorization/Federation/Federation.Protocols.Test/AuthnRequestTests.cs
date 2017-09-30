using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DeflateCompression;
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
        public async Task BuildAuthnRequest_test()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var authnRequestContext = new AuthnRequestContext(requestUri, "local");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var serialiser = new XMLSerialiser();
            var certManager = new CertificateManager();
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext, federationPartyContextBuilder);
            var compressor = new DeflateCompressor();
            //ACT
            var query = await AuthnRequestHelper.SerialiseAndSign(authnRequest, authnRequestContext, serialiser, federationPartyContextBuilder, certManager, compressor);
            var url = new Uri(String.Format("{0}?{1}", requestUri, query));
            var qsParsed = HttpUtility.ParseQueryString(url.Query);
            //request
            var requestEscaped = qsParsed[0];
            var requestUnescaped = Uri.UnescapeDataString(requestEscaped);
            var requestDecompressed = await Helper.DeflateDecompress(requestUnescaped, compressor);

            //relyingState
            var rsEscaped = qsParsed[1];
            var rsunescaped = Uri.UnescapeDataString(rsEscaped);
            var rsdecompressed = await Helper.DeflateDecompress(rsunescaped, compressor);
            
            //signature alg
            var sigAlgEscaped = qsParsed[2];
            var sigAlgunescaped = Uri.UnescapeDataString(sigAlgEscaped);

            //signature
            var sigEscaped = qsParsed[3];
            var sigunescaped = Uri.UnescapeDataString(sigEscaped);
            //ASSERT
        }

        [Test]
        public void SerialiseRequest()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var authnRequestContext = new AuthnRequestContext(requestUri, "local");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var serialiser = new XMLSerialiser();
            var certManager = new CertificateManager();
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext, federationPartyContextBuilder);

            //ACT
            var request = AuthnRequestHelper.Serialise(authnRequest, serialiser);
            //ASSERT
        }
    }
}
