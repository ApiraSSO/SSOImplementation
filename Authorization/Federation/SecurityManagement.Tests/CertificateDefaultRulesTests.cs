using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kernel.Cryptography.Validation;
using NUnit.Framework;
using SecurityManagement.CertificateValidationRules;

namespace SecurityManagement.Tests
{
    [TestFixture]
    internal class CertificateDefaultRulesTests
    {
        [Test]
        public async Task EffectiveDateRuleTest()
        {
            //ARRANGE
            var store = new X509Store("TestCertStore");
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certificate = store.Certificates.Find(X509FindType.FindBySubjectName, "ApiraTestCertificate", false)[0];
                var context = new CertificateValidationContext(certificate);
                var rule = new EffectiveDateRule();
                //ACT
                await rule.Validate(context, c => Task.CompletedTask);
                //ASSERT

            }
            finally
            {
                store.Close();
                store.Dispose();
            }
        }

        [Test]
        public async Task ExpirationDateRuleTest()
        {
            //ARRANGE
            var store = new X509Store("TestCertStore");
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certificate = store.Certificates.Find(X509FindType.FindBySubjectName, "ApiraTestCertificate", false)[0];
                var context = new CertificateValidationContext(certificate);
                var rule = new ExpirationDateRule();
                //ACT
                await rule.Validate(context, c => Task.CompletedTask);
                //ASSERT

            }
            finally
            {
                store.Close();
                store.Dispose();
            }
        }
    }
}
