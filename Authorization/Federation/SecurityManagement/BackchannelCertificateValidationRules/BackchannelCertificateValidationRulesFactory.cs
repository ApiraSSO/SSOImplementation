
using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Cryptography.Validation;
using Kernel.Reflection;

namespace SecurityManagement.BackchannelCertificateValidationRules
{
    internal class BackchannelCertificateValidationRulesFactory
    {
        public static IEnumerable<IBackchannelCertificateValidationRule> GetRules(CertificateValidationConfiguration configuration)
        {
            var rules = ReflectionHelper.GetAllTypes(new[] { typeof(BackchannelValidationRule).Assembly }, t =>
            !t.IsAbstract && !t.IsInterface && typeof(IBackchannelCertificateValidationRule).IsAssignableFrom(t))
            .Select(t => (IBackchannelCertificateValidationRule)Activator.CreateInstance(t));
            return rules;
        }
    }
}