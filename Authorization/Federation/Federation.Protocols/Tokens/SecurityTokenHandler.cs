using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Xml;
using Federation.Protocols.Tokens.Validation;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens
{
    internal class SecurityTokenHandler : Saml2SecurityTokenHandler, ITokenHandler, ITokenValidator
    {
        //ToDo: remove
        private readonly ITokenHandlerConfigurationProvider _tokenHandlerConfigurationProvider;
        //ToDo: remove
        private readonly ValidatorInvoker _validatorInvoker;

        private readonly ITokenSerialiser _tokenSerialiser;

        public SecurityTokenHandler(ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider, ValidatorInvoker validatorInvoker)
        {
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
            this._validatorInvoker = validatorInvoker;
        }
        public SecurityTokenHandler(ITokenSerialiser tokenSerialiser, ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider, ValidatorInvoker validatorInvoker)
        {
            this._tokenSerialiser = tokenSerialiser;
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

        public TokenHandlingResponse HandleToken(HandleTokenContext context)
        {
            throw new NotImplementedException();
        }

        public  SecurityToken ReadToken(XmlReader reader, string partnerId)
        {
            var token = this._tokenSerialiser.DeserialiseToken(reader, partnerId);
            return token;
        }

        public bool Validate(SecurityToken token, ICollection<ValidationResult> validationResult, string partnerId)
        {
            try
            {
                this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
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