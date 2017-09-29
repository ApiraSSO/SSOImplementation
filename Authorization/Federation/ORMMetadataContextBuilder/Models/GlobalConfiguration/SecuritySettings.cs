using System.Collections.Generic;
using System.ServiceModel.Security;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models.GlobalConfiguration
{
    public class SecuritySettings : BaseModel
    {
        public SecuritySettings()
        {
            this.CertificateValidationRules = new List<CertificateValidationRule>();
            this.RelyingParties = new List<FederationPartySettings>();
        }
        public X509CertificateValidationMode X509CertificateValidationMode { get; set; }
        public bool PinnedValidation { get; set; }
        public string PinnedTypeValidator { get; set; }
        public virtual ICollection<CertificateValidationRule> CertificateValidationRules { get; set; }
        public virtual ICollection<FederationPartySettings> RelyingParties { get; }
    }
}