using System.Collections.Generic;

namespace Kernel.Federation.MetaData.Configuration.Organisation
{
    public class ContactConfiguration
    {
        public ICollection<ContactPerson> PersonContact { get; set; }
        public ContactConfiguration()
        {
            this.PersonContact = new List<ContactPerson>();
        }
    }
}