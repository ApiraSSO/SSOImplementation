using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class PersonContact : BaseModel
    {
        public PersonContact()
        {
            this.Organisations = new List<OrganisationSettings>();
            this.Emails = new List<LocalisedName>();
            this.Phones = new List<Phone>();
        }
        public Kernel.Federation.MetaData.Configuration.Organisation.ContactType ContactType { get; set; }
        public virtual LocalisedName Forename { get; set; }
        public virtual LocalisedName Surname { get; set; }
        public virtual ICollection<LocalisedName> Emails { get; }
        public virtual ICollection<Phone> Phones { get; }
        public virtual ICollection<OrganisationSettings> Organisations { get; }
    }
}