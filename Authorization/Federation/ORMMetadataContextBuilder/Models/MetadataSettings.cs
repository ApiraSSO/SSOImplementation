using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class MetadataSettings: BaseModel
    {
        public MetadataSettings()
        {
            this.RelyingParties = new List<FederationPartySettings>();
        }
       public virtual SigningCredential SigningCredential { get; set; }
        public virtual EntityDescriptorSettings SPDescriptorSettings { get; set; }
        public virtual ICollection<FederationPartySettings> RelyingParties { get; }
    }
}
