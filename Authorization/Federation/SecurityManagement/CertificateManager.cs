﻿using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.DataProtection;
using Kernel.Federation.MetaData.Configuration.Cryptography;

namespace SecurityManagement
{
    public class CertificateManager : ICertificateManager
    {
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
            if (certContext is X509CertificateContext)
                return new X509StoreCertificateConfiguration(certContext);

            throw new NotSupportedException(String.Format("Certificate context of type: {0} is not supported.", certContext.GetType().Name));
        }

        public string SignToBase64(string dataToSign, CertificateContext certContext)
        {
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