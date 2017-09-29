using System.IdentityModel.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Federation.FederationPartner;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class ConfigurationRetrieverMock : IConfigurationRetriever<MetadataBase>
    {
        public Task<MetadataBase> GetAsync(FederationPartyContext context, CancellationToken cancel)
        {
            var metadata = this.GetMetadata();
            return Task.FromResult(metadata);
        }

        private MetadataBase GetMetadata()
        {
            return new EntityDescriptor();
        }
    }
}