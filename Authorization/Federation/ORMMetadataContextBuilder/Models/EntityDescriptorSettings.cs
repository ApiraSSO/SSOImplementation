using System;
using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class EntityDescriptorSettings : BaseModel
    {
        public EntityDescriptorSettings()
        {
            this.RoleDescriptors = new List<RoleDescriptorSettings>();
            this.IncludeOrganisationInfo = false;
        }

        public string EntityId { get; set; }
        public string FederationId { get; set; }
        public bool IncludeOrganisationInfo { get; set; }

        public DateTimeOffset ValidUntil { get; set; }
        public virtual DatepartValue CacheDuration { get; set; }
        public virtual OrganisationSettings Organisation { get; set; }
        public virtual ICollection<RoleDescriptorSettings> RoleDescriptors { get; }
    }
}