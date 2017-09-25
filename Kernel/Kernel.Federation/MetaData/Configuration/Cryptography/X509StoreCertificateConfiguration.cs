using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Kernel.Cryptography.CertificateManagement;

namespace Kernel.Federation.MetaData.Configuration.Cryptography
{
    public class X509StoreCertificateConfiguration : CertificateStore<X509Store>
    {
        private readonly CertificateContext _certificateContext;

        public X509StoreCertificateConfiguration(CertificateContext certificateContext)
            :base(new X509Store(((X509CertificateContext)certificateContext).StoreName, ((X509CertificateContext)certificateContext).StoreLocation))
        {
            if (certificateContext == null)
                throw new ArgumentNullException("certificateContext");

            this._certificateContext = certificateContext;
        }

        public override X509Certificate2 GetX509Certificate2()
        {
            if (this._certificateContext == null)
                throw new ArgumentNullException("certificateContext");
            
            using (base.Store)
            {
                base.Store.Open(OpenFlags.ReadOnly);
                var certificates = base.Store.Certificates;
                bool found = false;
                X509Certificate2 cert = null;
                var builder = new StringBuilder();

                foreach (var current in this._certificateContext.SearchCriteria)
                {
                    builder.AppendFormat("SearchCriteriaType: {0}, value: {1}/r/n", current.SearchCriteriaType, current.SearchValue);

                    var certs = certificates.Find(current.SearchCriteriaType, current.SearchValue, this._certificateContext.ValidOnly);
                    if (certs.Count != 1)
                        continue;

                    found = true;
                    cert = certs[0];
                    break;
                }
                if (!found)
                    throw new InvalidOperationException(String.Format("No certificate found. Search searches performed: {0}", builder.ToString()));
                return cert;
            }
        }
    }
}