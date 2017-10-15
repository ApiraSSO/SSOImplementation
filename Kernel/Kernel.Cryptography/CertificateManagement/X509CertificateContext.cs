using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;

namespace Kernel.Cryptography.CertificateManagement
{
    public class X509CertificateContext : CertificateContext
    {
        public string StoreName { get; set; }
        public StoreLocation StoreLocation { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            sb.AppendFormat("StoreName: {0}\r\n", this.StoreName);
            sb.AppendFormat("StoreLocation: {0}\r\n", this.StoreLocation);
            base.SearchCriteria.Aggregate(sb, (b, next) =>
            {
                b.AppendFormat("SearchCriteriaType: {0}\r\n", next.SearchCriteriaType);
                b.AppendFormat("SearchValue: {0}\r\n", next.SearchValue.ToString());
                return b;
            });
            return sb.ToString();
        }
    }
}