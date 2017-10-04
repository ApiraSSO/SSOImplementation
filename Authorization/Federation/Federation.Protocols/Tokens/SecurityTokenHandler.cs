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
    internal class SecurityTokenHandler : Saml2SecurityTokenHandler, ITokenHandler
    {
        //ToDo: remove
        private readonly ITokenHandlerConfigurationProvider _tokenHandlerConfigurationProvider;
        //ToDo: remove
        private readonly ValidatorInvoker _validatorInvoker;

        private readonly ITokenSerialiser _tokenSerialiser;
        private readonly ITokenValidator _tokenValidator;

        public SecurityTokenHandler(ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider, ValidatorInvoker validatorInvoker)
        {
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
            this._validatorInvoker = validatorInvoker;
        }
        public SecurityTokenHandler(ITokenSerialiser tokenSerialiser, ITokenValidator tokenValidator, ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider, ValidatorInvoker validatorInvoker)
        {
            this._tokenSerialiser = tokenSerialiser;
            this._tokenValidator = tokenValidator;
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
            this._validatorInvoker = validatorInvoker;
        }
        //internal IEnumerable<ClaimsIdentity> Claims { get; private set; }

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
            return this._tokenValidator.Validate(token, validationResult, partnerId);
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