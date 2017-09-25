using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class Protocol : BaseModel
    {
        public Protocol()
        {
            this.RoleDescriptors = new List<RoleDescriptorSettings>();
        }
        public string Uri { get; set; }
        public virtual ICollection<RoleDescriptorSettings> RoleDescriptors { get; set; }
    }
}