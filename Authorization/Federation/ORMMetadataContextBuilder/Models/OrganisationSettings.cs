using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class OrganisationSettings : BaseModel
    {
        public OrganisationSettings()
        {
            this.Names = new List<LocalisedName>();
            this.Urls = new List<LocalisedName>();
            this.Contacts = new List<PersonContact>();
        }
        public virtual ICollection<LocalisedName> Names { get; }
        public virtual ICollection<LocalisedName> Urls { get; }

        public virtual ICollection<PersonContact> Contacts { get; }
    }
}