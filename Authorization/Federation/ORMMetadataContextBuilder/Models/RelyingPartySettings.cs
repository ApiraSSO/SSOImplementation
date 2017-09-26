using Kernel.Data;
using ORMMetadataContextProvider.Models.GlobalConfiguration;

namespace ORMMetadataContextProvider.Models
{
    public class RelyingPartySettings : BaseModel
    {
        public string RelyingPartyId { get; set; }
        public string MetadataPath { get; set; }
        public string MetadataLocation { get; set; }
        public int AutoRefreshInterval { get; set; }
        public int RefreshInterval { get; set; }
        public virtual SecuritySettings SecuritySettings { get; set; }
        public virtual MetadataSettings MetadataSettings { get; set; }
    }
}