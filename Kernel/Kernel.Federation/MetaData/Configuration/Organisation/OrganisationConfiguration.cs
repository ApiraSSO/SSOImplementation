using System.Collections.Generic;

namespace Kernel.Federation.MetaData.Configuration.Organisation
{
    public class OrganisationConfiguration
    {
        public ICollection<LocalizedConfigurationEntry> Names { get; }
        public ContactConfiguration OrganisationContacts { get; set; }
        public ICollection<LocalizedUrlEntry> Urls { get; }
        public OrganisationConfiguration()
        {
            this.Urls = new List<LocalizedUrlEntry>();
            this.Names = new List<LocalizedConfigurationEntry>();
        }
    }
}