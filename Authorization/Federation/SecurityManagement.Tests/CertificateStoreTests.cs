using System;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using NUnit.Framework;

namespace SecurityManagement.Tests
{
    [TestFixture]
    internal class CertificateStoreTests
    {
        [Test]
        public void X509CertStoreConfigurationTestTest_should_pass_subject_localMachine_invalid_included()
        {
            //ARRANGE
            var certificateContext = new X509CertificateContext
            {
                StoreName = "TestCertStore",
                ValidOnly = false,
                StoreLocation = StoreLocation.LocalMachine
            };
            certificateContext.SearchCriteria.Add(new CertificateSearchCriteria { SearchValue = "ApiraTestCertificate", SearchCriteriaType = X509FindType.FindBySubjectName });
            var certConfiguration = new X509StoreCertificateConfiguration(certificateContext);
            //ACT
            X509Certificate2 cert = null;
            //ASSERT
            Assert.DoesNotThrow(() => cert = certConfiguration.GetX509Certificate2());
            Assert.NotNull(cert);
        }

        [Test]
        public void X509CertStoreConfigurationTestTest_should_pass_subject_current_user_invalid_included()
        {
            //ARRANGE
            var certificateContext = new X509CertificateContext
            {
                StoreName = "TestCertStore",
                ValidOnly = false,
                StoreLocation = StoreLocation.CurrentUser
            };
            certificateContext.SearchCriteria.Add(new CertificateSearchCriteria { SearchValue = "ApiraTestCertificate", SearchCriteriaType = X509FindType.FindBySubjectName });
            var certConfiguration = new X509StoreCertificateConfiguration(certificateContext);
            //ACT
            X509Certificate2 cert = null;
            //ASSERT
            Assert.DoesNotThrow(() => cert = certConfiguration.GetX509Certificate2());
            Assert.NotNull(cert);
        }

        [Test]
        public void X509CertStoreConfigurationTestTest_should_fail_subject_localMachine_valid_only()
        {
            //ARRANGE
            var certificateContext = new X509CertificateContext
            {
                StoreName = "TestCertStore",
               
                ValidOnly = true,
                
                StoreLocation = StoreLocation.LocalMachine
            };
            certificateContext.SearchCriteria.Add(new CertificateSearchCriteria { SearchValue = "ApiraTestCertificate", SearchCriteriaType = X509FindType.FindBySubjectName });
            var certConfiguration = new X509StoreCertificateConfiguration(certificateContext);
            //ACT
            X509Certificate2 cert = null;
            //ASSERT
            Assert.Throws<InvalidOperationException>(() => cert = certConfiguration.GetX509Certificate2());
        }
    }
}