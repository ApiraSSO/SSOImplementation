using System.Linq;
using Kernel.Cryptography.Validation;
using NUnit.Framework;
using SecurityManagement.CertificateValidationRules;
using SecurityManagement.Tests.Mock;

namespace SecurityManagement.Tests
{
    [TestFixture]
    internal class CertificateValidationRulesFactoryTest
    {
        [Test]
        public void GetRules_default_ones()
        {
            //ARRANGE
            var configuration = new CertificateValidationConfiguration
            {
                UsePinningValidation = false,
                X509CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom
            };

            //ACT
            var rules = CertificateValidationRulesFactory.GetRules(configuration)
                .ToList();
            //ASSERT
            Assert.AreEqual(2, rules.Count);
            Assert.IsTrue(rules.Any(x => x.GetType() == typeof(EffectiveDateRule)));
            Assert.IsTrue(rules.Any(x => x.GetType() == typeof(ExpirationDateRule)));
        }

        [Test]
        public void GetRules_default_ones_and_injected()
        {
            //ARRANGE
            var configuration = new CertificateValidationConfiguration
            {
                UsePinningValidation = false,
                X509CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom
            };
            var rule1 = typeof(CertificateValidationRuleMock1).AssemblyQualifiedName;
            var rule2 = typeof(CertificateValidationRuleMock).AssemblyQualifiedName;
            var rule3 = typeof(CertificateValidationRuleFailedMock).AssemblyQualifiedName;
            var ruleDescriptor = new ValidationRuleDescriptor(rule1);
            var ruleDescriptor2 = new ValidationRuleDescriptor(rule2);
            var ruleDescriptor3 = new ValidationRuleDescriptor(rule3);
            configuration.ValidationRules.Add(ruleDescriptor);
            configuration.ValidationRules.Add(ruleDescriptor2);
            configuration.ValidationRules.Add(ruleDescriptor3);
            //ACT
            var rules = CertificateValidationRulesFactory.GetRules(configuration)
                .ToList();
            //ASSERT
            Assert.AreEqual(5, rules.Count);
            Assert.IsTrue(rules.Any(x => x.GetType() == typeof(EffectiveDateRule)));
            Assert.IsTrue(rules.Any(x => x.GetType() == typeof(ExpirationDateRule)));
            Assert.IsTrue(rules.Any(x => x.GetType() == typeof(CertificateValidationRuleMock1)));
            Assert.IsTrue(rules.Any(x => x.GetType() == typeof(CertificateValidationRuleMock)));
            Assert.IsTrue(rules.Any(x => x.GetType() == typeof(CertificateValidationRuleFailedMock)));
        }
    }
}