using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Data;
using Kernel.Federation.MetaData.Configuration.Organisation;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace Kernel.Federation.MetaData.Configuration.EntityDescriptors
{
    public class EntityDesriptorConfiguration : IHasID<string>
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public DateTimeOffset ValidUntil { get; set; }
        public TimeSpan CacheDuration { get; set; }
        public OrganisationConfiguration Organisation { get; set; }
        public ICollection<RoleDescriptorConfiguration> RoleDescriptors { get; }
        
        public ICollection<SPSSODescriptorConfiguration> SPSSODescriptors
        {
            get
            {
                return this.RoleDescriptors.OfType<SPSSODescriptorConfiguration>()
                    .ToList();
            }
        }
        public EntityDesriptorConfiguration()
        {
            this.RoleDescriptors = new List<RoleDescriptorConfiguration>();
        }
    }
}