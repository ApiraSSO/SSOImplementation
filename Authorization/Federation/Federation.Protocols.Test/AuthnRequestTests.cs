using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DeflateCompression;
using Federation.Protocols.Request;
using Federation.Protocols.Test.Mock;
using Kernel.Cryptography.CertificateManagement;
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
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local");
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex);
            
            var serialiser = new XMLSerialiser();
            var certManager = new CertificateManager();
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);
            var compressor = new DeflateCompressor();
            var certContext = new X509CertificateContext
            {
                StoreLocation = StoreLocation.LocalMachine,
                ValidOnly = false,
                StoreName = "TestCertStore"
            };
            certContext.SearchCriteria.Add(new CertificateSearchCriteria
            {
                SearchCriteriaType = X509FindType.FindBySubjectName,
                SearchValue = "ApiraTestCertificate"
            });
            //ACT
            var sb = new StringBuilder();
            //serialise and append request
            var serialised =  AuthnRequestHelper.Serialise(authnRequest, serialiser);
            await AuthnRequestHelper.AppendRequest(sb, serialised, compressor);

            //append relying state
            await AuthnRequestHelper.AppendRelyingState(sb, authnRequestContext, compressor);

            //append signature alg
            AuthnRequestHelper.AppendSignarureAlgorithm(sb);

            var datataToSign = sb.ToString();

            //sign request
            AuthnRequestHelper.SignData(sb, certContext, certManager);
            var query = sb.ToString();
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

            //signed string
            var verified = certManager.VerifySignatureFromBase64(datataToSign, sigEscaped, certContext);
            //ASSERT
            Assert.True(verified);
        }

        [Test]
        public void SerialiseRequest()
        {
            //ARRANGE
            var requestUri = new Uri("http://localhost:59611/");
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var federationContex = federationPartyContextBuilder.BuildContext("local");
            var authnRequestContext = new AuthnRequestContext(requestUri, federationContex);
            
            var serialiser = new XMLSerialiser();
            var certManager = new CertificateManager();
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext);

            //ACT
            var request = AuthnRequestHelper.Serialise(authnRequest, serialiser);
            //ASSERT
        }
    }
}
