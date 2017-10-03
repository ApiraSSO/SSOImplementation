using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Xml;
using Federation.Protocols.Tokens.Validation;
using Kernel.Cryptography.Validation;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens
{
    internal class SecurityTokenHandler : Saml2SecurityTokenHandler, ITokenHandler, ITokenValidator
    {
        private readonly ITokenHandlerConfigurationProvider _tokenHandlerConfigurationProvider;
        private readonly ValidatorInvoker _validatorInvoker;

        public SecurityTokenHandler(ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider, ValidatorInvoker validatorInvoker)
        {
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
            this._validatorInvoker = validatorInvoker;
        }
        internal IEnumerable<ClaimsIdentity> Claims { get; private set; }

        public Saml2Assertion GetAssertion(XmlReader reader, string partnerId)
        {
            this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
            TokenHelper.MoveToToken(reader);

            return this.ReadAssertion(reader);
        }

        public  SecurityToken ReadToken(XmlReader reader, string partnerId)
        {
            this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
            TokenHelper.MoveToToken(reader);
            return base.ReadToken(reader);
        }

        public bool Validate(SecurityToken token, ICollection<ValidationResult> validationResult, string partnerId)
        {
            try
            {
                ((ICertificateValidator)base.CertificateValidator).SetFederationPartyId(partnerId);
                this.Claims = base.ValidateToken(token);
                return true;
            }
            catch (Exception ex)
            {
                validationResult.Add(new ValidationResult(ex.Message));
                return false;
            }
        }

        protected override void ValidateConfirmationData(Saml2SubjectConfirmationData confirmationData)
        {
            this._validatorInvoker.Validate(confirmationData);
        }
        
        internal void SetConfigurationFor(string partnerId)
        {
            this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
        }
    }
}