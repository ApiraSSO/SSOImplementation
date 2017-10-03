using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;

namespace Kernel.Federation.Protocols.Response
{
    public interface ITokenValidator
    {
        bool Validate(SecurityToken token, ICollection<ValidationResult> validationResult, string partnerId);
    }
}