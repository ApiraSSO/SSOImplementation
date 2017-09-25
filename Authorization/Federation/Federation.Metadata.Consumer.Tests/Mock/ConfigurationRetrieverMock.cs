using System.IdentityModel.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Federation.RelyingParty;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class ConfigurationRetrieverMock : IConfigurationRetriever<MetadataBase>
    {
        public Task<MetadataBase> GetAsync(string address, CancellationToken cancel)
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