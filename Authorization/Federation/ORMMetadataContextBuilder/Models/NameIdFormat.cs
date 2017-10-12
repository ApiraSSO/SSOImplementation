using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class NameIdFormat : BaseModel
    {
        public NameIdFormat()
        {
            this.SSODescriptorSettings = new List<SSODescriptorSettings>();
        }

        public string Key { get; set; }
        public string Uri { get; set; }
        public virtual ICollection<SSODescriptorSettings> SSODescriptorSettings { get; }
    }
}