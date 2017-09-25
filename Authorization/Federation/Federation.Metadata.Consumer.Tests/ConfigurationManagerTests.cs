using System.IdentityModel.Metadata;
using System.Threading.Tasks;
using Federation.Metadata.Consumer.Tests.Mock;
using Federation.Metadata.RelyingParty.Configuration;
using Kernel.Federation.RelyingParty;
using NUnit.Framework;

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
            var relyingPartyId = "imperial.ac.uk";
            var relyingPartyContextBuilder = new RelyingPartyContextBuilderMock();
            var configurationRetriever = new ConfigurationRetrieverMock();
            var configurationManager = new ConfigurationManager<MetadataBase>(relyingPartyContextBuilder, configurationRetriever);
            //ACT
            configuration = await configurationManager.GetConfigurationAsync(relyingPartyId);
            //ASSET
            Assert.IsNotNull(configuration);
        }
    }
}