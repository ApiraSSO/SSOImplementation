using Kernel.Cryptography.Validation;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models.GlobalConfiguration
{
    public class CertificateValidationRule : BaseModel
    {
        public string Type { get; set; }
        public virtual ValidationScope Scope { get; set; }
    }
}