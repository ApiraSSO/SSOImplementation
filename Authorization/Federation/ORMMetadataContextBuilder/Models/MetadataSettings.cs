using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class MetadataSettings: BaseModel
    {
       public virtual SigningCredential SigningCredential { get; set; }
        public virtual EntityDescriptorSettings SPDescriptorSettings { get; set; }
    }
}
