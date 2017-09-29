using System.IdentityModel.Metadata;
using System.Threading.Tasks;
using Federation.Metadata.Consumer.Tests.Mock;
using NUnit.Framework;
using Shared.Federtion;

namespace Federation.Metadata.Consumer.Tests
{
    [TestFixture]
    internal class ConfigurationManagerTests
    {
        [Test]
        public async Task ManagerTest()
        {
            //ARRANGE
            MetadataBase configuration = null;
            var federationPartyId = "imperial.ac.uk";
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var configurationRetriever = new ConfigurationRetrieverMock();
            var configurationManager = new ConfigurationManager<MetadataBase>(federationPartyContextBuilder, configurationRetriever);
            //ACT
            configuration = await configurationManager.GetConfigurationAsync(federationPartyId);
            //ASSET
            Assert.IsNotNull(configuration);
        }
    }
}