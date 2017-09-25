using System.Collections.Generic;

namespace Kernel.Cryptography.CertificateManagement
{
    public class CertificateContext
    {
        public CertificateContext()
        {
            this.SearchCriteria = new List<CertificateSearchCriteria>();
        }
        public ICollection<CertificateSearchCriteria> SearchCriteria { get; }
        public bool ValidOnly { get; set; }
    }
}