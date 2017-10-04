using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace Kernel.Federation.Tokens
{
    public class TokenHandlingResponse
    {
        private bool _siValid;
        public TokenHandlingResponse(SecurityToken token, ClaimsIdentity identity, ICollection<ValidationResult> validationResults)
        {
            this.Token = token;
            this.Identity = identity;
            this.ValidationResults = validationResults;
        }
        public ICollection<ValidationResult> ValidationResults { get; }
        public bool IsValid
        {
            get
            {
                return this._siValid && this.ValidationResults != null && this.ValidationResults.Count == 0;
            }
        }
        public ClaimsIdentity Identity { get; }
        public SecurityToken Token { get; }
    }
}