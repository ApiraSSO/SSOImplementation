using System;
using System.IdentityModel.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Federation.FederationPartner;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class ConfigurationRetrieverMock : IConfigurationRetriever<MetadataBase>
    {
        public Action<MetadataBase> MetadataReceivedCallback { get; set; }

        public Task<MetadataBase> GetAsync(FederationPartyConfiguration context, CancellationToken cancel)
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