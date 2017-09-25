using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Kernel.Data;
using Kernel.Federation.MetaData.Configuration.Cryptography;

namespace ORMMetadataContextProvider.Models
{
    public class Certificate : BaseModel
    {
        public Certificate()
        {
            this.StoreSearchCriteria = new List<StoreSearchCriterion>();
            this.SigningCredentials = new List<SigningCredential>();
            this.RoleDescriptors = new List<RoleDescriptorSettings>();
        }
        public string CertificatePath { get; }
        public string CertificatePKPath { get; set; }
        public string CertificatePfxPath { get; set; }
        public string Password { get; set; }
        public string CetrificateStore { get; set; }
        public StoreLocation StoreLocation { get; set; }
        public KeyUsage Use { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<StoreSearchCriterion> StoreSearchCriteria { get; }
        public virtual ICollection<SigningCredential> SigningCredentials { get; set; }
        public virtual ICollection<RoleDescriptorSettings> RoleDescriptors{ get; set; }
    }
}