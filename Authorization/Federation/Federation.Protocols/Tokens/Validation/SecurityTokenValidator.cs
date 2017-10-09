using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using Kernel.Federation.Tokens;
using Kernel.Logging;

namespace Federation.Protocols.Tokens.Validation
{
    internal class SecurityTokenValidator : Saml2SecurityTokenHandler, ITokenValidator
    {
        private readonly ITokenConfigurationProvider<SecurityTokenHandlerConfiguration> _tokenHandlerConfigurationProvider;
        
        private readonly ValidatorInvoker _validatorInvoker;
        
        public SecurityTokenValidator(ITokenConfigurationProvider<SecurityTokenHandlerConfiguration> tokenHandlerConfigurationProvider, ValidatorInvoker validatorInvoker)
        {
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
            this._validatorInvoker = validatorInvoker;
        }
       
        public bool Validate(SecurityToken token, ICollection<ValidationResult> validationResult, string partnerId)
        {
            try
            {
                var configuration = this._tokenHandlerConfigurationProvider.GetConfiguration(partnerId);
                base.CertificateValidator = configuration.CertificateValidator;
                base.Configuration = configuration;
                var claims = base.ValidateToken(token);
                return true;
            }
            catch (Exception ex)
            {
                validationResult.Add(new ValidationResult(ex.Message));
                LoggerManager.WriteExceptionToEventLog(ex);
                return false;
            }
        }

        protected override void ValidateConfirmationData(Saml2SubjectConfirmationData confirmationData)
        {
            this._validatorInvoker.Validate(confirmationData);
        }
    }
}