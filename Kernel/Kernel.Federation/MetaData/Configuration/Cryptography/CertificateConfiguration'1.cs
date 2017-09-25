namespace Kernel.Federation.MetaData.Configuration.Cryptography
{
    public abstract class CertificateConfiguration<TStore> : CertificateConfiguration
    {
        public TStore Store { get; }
        
        public CertificateConfiguration(TStore store)
        {
            this.Store = store;
        }
    }
}