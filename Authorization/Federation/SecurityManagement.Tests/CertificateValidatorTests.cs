using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using SecurityManagement.Tests.Mock;

namespace SecurityManagement.Tests
{
    [TestFixture]
    
    public class CertificateValidatorTests
    {
        [Test]
        
        public void MetadataSerialisationCertificateTest()
        {
            //ARRANGE
            var configurationProvider = new CertificateValidationConfigurationProvider();
            var validator = new CertificateValidator(configurationProvider);
            //ACT

            //ASSERT
            Assert.Throws<NotImplementedException>(() => validator.Validate((X509Certificate2)null));
        }

        [Test]
        public void RemoteCertificateValidationCallbackTest()
        {
            //ARRANGE
            var configurationProvider = new CertificateValidationConfigurationProvider();
            var validator = new CertificateValidator(configurationProvider);
            //ACT

            //ASSERT
            Assert.Throws<NotImplementedException>(() => validator.Validate(null, null, null, System.Net.Security.SslPolicyErrors.None));
        }

        [Test]
        public void RemoteCertificateValidationRulesTest()
        {
            //ARRANGE
            var configurationProvider = new CertificateValidationConfigurationProvider();
            var validator = new CertificateValidator(configurationProvider);

            var certificateStore = new X509Store("TestCertStore", StoreLocation.LocalMachine);
            var validationResult = false;
            //ACT
            try
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                var certificate = certificateStore.Certificates.Find(X509FindType.FindBySubjectName, "ApiraTestCertificate", false)[0];
                var x509Chain = new X509Chain(true);
                x509Chain.Build(certificate);
                validationResult = validator.Validate(this, certificate, x509Chain, SslPolicyErrors.None);
            }
            finally
            {
                certificateStore.Close();
                certificateStore.Dispose();
            }
            //ASSERT
            Assert.True(validationResult);
        }
    }
}