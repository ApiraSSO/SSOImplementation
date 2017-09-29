using System.IO;

namespace Kernel.Federation.MetaData
{
    public class MetadataGenerateRequest
    {
        public MetadataGenerateRequest(MetadataType type, string federationPartyId)
            :this(type, federationPartyId, new MetadataPublishContext(new MemoryStream(), MetadataPublishProtocol.Memory))
        {
            
        }
        public MetadataGenerateRequest(MetadataType type, string federationPartyId, MetadataPublishContext target)
        {
            this.MetadataType = type;
            this.FederationPartyId = federationPartyId;
            this.Target = target;
        }

        public MetadataType MetadataType { get; }
        public string FederationPartyId { get; set; }
        public MetadataPublishContext Target { get; }
    }
}