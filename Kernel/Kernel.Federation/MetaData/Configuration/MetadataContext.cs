using Kernel.Federation.MetaData.Configuration.Cryptography;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;

namespace Kernel.Federation.MetaData.Configuration
{
    public class MetadataContext
    {
        public bool SignMetadata { get; set; }
        public string EntityId { get { return this.EntityDesriptorConfiguration.EntityId; } }
        public MetadataSigningContext MetadataSigningContext { get; set; }
        public EntityDesriptorConfiguration EntityDesriptorConfiguration { get; set; }

        public MetadataContext()
        {
            this.SignMetadata = true;
        }
    }
}