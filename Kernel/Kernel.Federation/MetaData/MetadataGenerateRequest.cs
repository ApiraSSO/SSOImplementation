namespace Kernel.Federation.MetaData
{
    public class MetadataGenerateRequest
    {
        public MetadataGenerateRequest(MetadataType type, string federationPartyId)
        {
            this.MetadataType = type;
            this.FederationPartyId = federationPartyId;
        }
        public MetadataType MetadataType { get; }
        public string FederationPartyId { get; set; }
    }
}
