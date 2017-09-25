using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class ConsumerServiceContext : IConsumerServiceContext
    {
        public int Index { get; set; }

        public bool IsDefault { get; set; }

        public string Location { get; set; }

        public string Binding { get; set; }
    }
}