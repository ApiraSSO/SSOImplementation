using System;
using Federation.Protocols.Request;
using Kernel.Federation.Protocols;
using NUnit.Framework;
using SecurityManagement;

namespace WsFederationMetadataProviderTests
{
    [TestFixture]
    internal class AuthnRequestTests
    {
        [Test]
        public void RedirectUriBuildTest()
        {
            throw new NotImplementedException();
            //ARRANGE
            //var certManager = new CertificateManager();
            //var requestContext = new AuthnRequestContext(null, new Uri("https://dg-mfb/idp/profile/SAML2/Redirect/SSO"));
            //var requestBuilder = new AuthnRequestBuilder(certManager);
            ////ACT
            //var uri = requestBuilder.BuildRedirectUri(requestContext);
            //var query = uri.Query;
            //ASSERT
        }
    }
}