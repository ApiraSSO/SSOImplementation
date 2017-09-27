using System;
using System.Collections.Generic;
using System.ServiceModel.Security;
using Kernel.Data;

namespace Kernel.Cryptography.Validation
{
    public class CertificateValidationConfiguration
    {
        public CertificateValidationConfiguration()
        {
            this.ValidationRules = new List<ValidationRuleDescriptor>();
        }
        public X509CertificateValidationMode X509CertificateValidationMode { get; set; }
        public TypeDescriptor BackchannelValidatorResolver { get; set; }
        public bool UsePinningValidation { get; set; }
        public ICollection<ValidationRuleDescriptor> ValidationRules { get; }
    }
}