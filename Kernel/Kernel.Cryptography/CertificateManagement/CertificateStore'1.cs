using System.Security.Cryptography.X509Certificates;

namespace Kernel.Cryptography.CertificateManagement
{
    public abstract class CertificateStore<TStore> : ICertificateStore
    {
        public TStore Store { get; }

        public StoreLocation StoreLocation { get; protected set; }

        public CertificateStore(TStore store)
        {
            this.Store = store;
            this.StoreLocation = StoreLocation.CurrentUser;
        }

        public abstract X509Certificate2 GetX509Certificate2();
    }
}