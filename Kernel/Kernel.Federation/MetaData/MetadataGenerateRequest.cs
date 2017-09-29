using System.IO;

namespace Kernel.Federation.MetaData
{
    public class MetadataGenerateRequest
    {
        public MetadataGenerateRequest(MetadataType type, string federationPartyId)
            :this(type, federationPartyId, new MemoryStream())
        {
            
        }
        public MetadataGenerateRequest(MetadataType type, string federationPartyId, Stream targetStream)
        {
            this.MetadataType = type;
            this.FederationPartyId = federationPartyId;
            this.TargetStream = targetStream;
        }

        public MetadataType MetadataType { get; }
        public string FederationPartyId { get; set; }
        public Stream TargetStream { get; }
    }
}
