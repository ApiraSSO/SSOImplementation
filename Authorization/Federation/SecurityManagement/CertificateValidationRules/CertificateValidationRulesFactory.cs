
using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Cryptography.Validation;
using Kernel.Reflection;

namespace SecurityManagement.CertificateValidationRules
{
    internal class CertificateValidationRulesFactory
    {
        public static IEnumerable<ICertificateValidationRule> GetRules(CertificateValidationConfiguration configuration)
        {
            var rules = ReflectionHelper.GetAllTypes(new[] { typeof(CertificateValidationRule).Assembly }, t =>
            !t.IsAbstract && !t.IsInterface && typeof(ICertificateValidationRule).IsAssignableFrom(t))
            .Union(configuration.ValidationRules.Select(x => x.Type))
            .Select(t => (ICertificateValidationRule)Activator.CreateInstance(t));
            return rules;
        }
    }
}