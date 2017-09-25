using System.Collections.Generic;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class SigningCredential : BaseModel
    {
        public SigningCredential()
        {
            this.Certificates = new List<Certificate>();
        }
        public string DigestAlgorithm { get; set; }

        public string SignatureAlgorithm { get; set; }
        public virtual ICollection<Certificate> Certificates { get; }
    }
}