using System;
using System.IdentityModel.Metadata;
using System.Net.Http;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Federation.Metadata.Consumer.Tests.Mock;
using Federation.Metadata.FederationPartner.Configuration;
using Federation.Metadata.HttpRetriever;
using Kernel.Federation.FederationPartner;
using NUnit.Framework;
using SecurityManagement;
using WsMetadataSerialisation.Serialisation;

namespace Federation.Metadata.Consumer.Tests
{
    [TestFixture]
    public class MetadataRetrieverMetadataTests
    {
        [Test]
        public async Task HttpDocumentRetrieverTest()
        {
            //ARRANGE
            var webRequestHandler = new WebRequestHandler();
            webRequestHandler.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((_, __, ___, ____) => true);
            var httpClient = new HttpClient(webRequestHandler);
            var documentRetrieer = new HttpDocumentRetriever(() => httpClient);

            //ACT
            
            //var document = await documentRetrieer.GetDocumentAsync("https://dg-mfb/idp/shibboleth", new CancellationToken());
            var document = await documentRetrieer.GetDocumentAsync("https://www.testshib.org/metadata/testshib-providers.xml", new CancellationToken());
            //ASSERT
            Assert.IsFalse(String.IsNullOrWhiteSpace(document));
        }

        [Test]
        public async Task WsFederationConfigurationRetrieverTest()
        {
            //ARRANGE
            var webRequestHandler = new WebRequestHandler();
            webRequestHandler.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((_, __, ___, ____) => true);
            var httpClient = new HttpClient(webRequestHandler);
            var documentRetrieer = new HttpDocumentRetriever(() => httpClient);
            var configurationProvider = new CertificateValidationConfigurationProvider();
            var certValidator = new CertificateValidator(configurationProvider);
            var logger = new LogProviderMock();
            var serialiser = new FederationMetadataSerialiser(certValidator, logger);
            var configurationRetriever = new WsFederationConfigurationRetriever(documentRetrieer, serialiser);



            //ACT
            //var baseMetadata = await WsFederationConfigurationRetriever.GetAsync("https://dg-mfb/idp/shibboleth", documentRetrieer, new CancellationToken());
            var context = new FederationPartyConfiguration("local", "https://www.testshib.org/metadata/testshib-providers.xml");
            var baseMetadata = await configurationRetriever.GetAsync(context, new CancellationToken());
            var metadata = baseMetadata as EntitiesDescriptor;
            //ASSERT
            Assert.IsTrue(metadata != null);
            Assert.AreEqual(2, metadata.ChildEntities.Count);
        }
    }
}