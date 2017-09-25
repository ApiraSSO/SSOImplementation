using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class RelyingPartySettings : BaseModel
    {
        public string RelyingPartyId { get; set; }
        public string MetadataPath { get; set; }
        public string MetadataLocation { get; set; }
        public int AutoRefreshInterval { get; set; }
        public int RefreshInterval { get; set; }
    }
}