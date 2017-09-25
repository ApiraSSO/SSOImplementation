using System;
using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class RoleDescriptorSettings : BaseModel
    {
        public RoleDescriptorSettings()
        {
            this.Protocols = new List<Protocol>();
            this.Certificates = new List<Certificate>();
        }
        public DateTimeOffset ValidUntil { get; set; }
        public virtual DatepartValue CacheDuration { get; set; }
        public string ErrorUrl { get; set; }
        public virtual ICollection<Protocol> Protocols { get; }
        public virtual ICollection<Certificate> Certificates { get; }
    }
}