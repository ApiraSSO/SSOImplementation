namespace Kernel.Federation.MetaData
{
    public class MetadataGenerateRequest
    {
        public MetadataGenerateRequest(MetadataType type, string relyingPartyId)
        {
            this.MetadataType = type;
            this.RelyingPartyId = relyingPartyId;
        }
        public MetadataType MetadataType { get; }
        public string RelyingPartyId { get; set; }
    }
}
