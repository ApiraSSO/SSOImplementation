using System;
using System.Net.Http;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Federation.Metadata.HttpRetriever.Test.Mock;
using Kernel.Cryptography.Validation;
using NUnit.Framework;

namespace Federation.Metadata.HttpRetriever.Test
{
    [TestFixture]
    public class HttpDocumentRetrieverTests
    {
        [Test]
        public async Task SecureChanelTestShibMetadata_load()
        {
            //ARRANGE
            var certValidator = new CertificateValidatorMock();
            var httpHandler = HttpDocumentRetrieverTests.ResolveHttpMessageHandler(certValidator);
            var documentRetriever = new HttpDocumentRetriever(() => new HttpClient(httpHandler));
            var address = "https://www.testshib.org/metadata/testshib-providers.xml";
            //ACT
            var doc = await documentRetriever.GetDocumentAsync(address, CancellationToken.None);
            //ASSERT
            Assert.False(String.IsNullOrWhiteSpace(doc));

        }

        private static HttpMessageHandler ResolveHttpMessageHandler(IBackchannelCertificateValidator validator)
        {
            HttpMessageHandler httpMessageHandler = new WebRequestHandler();
            //if (options.BackchannelCertificateValidator != null)
            {
                WebRequestHandler webRequestHandler = httpMessageHandler as WebRequestHandler;
                if (webRequestHandler == null)
                    throw new InvalidOperationException();
                webRequestHandler.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(validator.Validate);
            }
            return httpMessageHandler;
        }
    }
}