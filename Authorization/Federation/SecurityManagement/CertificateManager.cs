using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.DataProtection;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using Kernel.Logging;

namespace SecurityManagement
{
    public class CertificateManager : ICertificateManager
    {
        private readonly ILogProvider _logProvider;
        public CertificateManager(ILogProvider logProvider)
        {
            this._logProvider = logProvider;
        }
        public bool TryAddCertificateToStore(string storeName, StoreLocation location, X509Certificate2 certificate, bool createIfNotExist)
        {
            try
            {
                using (var store = new X509Store(storeName, location))
                {
                    var flags = OpenFlags.ReadWrite;
                    if (!createIfNotExist)
                        flags &= OpenFlags.OpenExistingOnly;
                    store.Open(flags);
                    store.Add(certificate);
                    return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }

        public X509Certificate2 GetCertificate(string path, SecureString password)
        {
            return new X509Certificate2(path, password);
        }

        public X509Certificate2 GetCertificate(ICertificateStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            return store.GetX509Certificate2();
        }

        public X509Certificate2 GetCertificateFromContext(CertificateContext certContext)
        { 
            var store = this.GetStoreFromContext(certContext);
            return this.GetCertificate(store);
        }

        public ICertificateStore GetStoreFromContext(CertificateContext certContext)
        {
            var sb = new StringBuilder();
            var x509Context = certContext as X509CertificateContext;
            if (x509Context == null)
            {
                sb.AppendFormat("Certificate context of type: {0} is not supported.", certContext.GetType().Name);
                throw new NotSupportedException(sb.ToString());
            }

            sb.AppendLine("Try to get certificate store from context. Certificate context details:");
            sb.AppendLine(x509Context.ToString());
            this._logProvider.LogMessage(sb.ToString());
            
            return new X509StoreCertificateConfiguration(x509Context);
        }

        public string SignToBase64(string dataToSign, CertificateContext certContext)
        {
            this._logProvider.LogMessage(String.Format("Signing data with certificate from context: {0}", certContext.ToString()));
            var data = Encoding.UTF8.GetBytes(dataToSign);
            var cert = this.GetCertificateFromContext(certContext);
            var signed = RSADataProtection.SignDataSHA1((RSA)cert.PrivateKey, data);

            var base64 = Convert.ToBase64String(signed);
            return base64;
        }

        public bool VerifySignatureFromBase64(string data, string signed, CertificateContext certContext)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signedBytes = Convert.FromBase64String(signed);
            
            var cert = this.GetCertificateFromContext(certContext);
            var verified = RSADataProtection.VerifyDataSHA1Signed((RSA)cert.PrivateKey, dataBytes, signedBytes);
            return verified;
        }
    }
}