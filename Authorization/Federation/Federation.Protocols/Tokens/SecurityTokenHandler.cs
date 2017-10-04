using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Xml;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens
{
    internal class SecurityTokenHandler : ITokenHandler
    {
        private readonly ITokenSerialiser _tokenSerialiser;
        private readonly ITokenValidator _tokenValidator;
        
        public SecurityTokenHandler(ITokenSerialiser tokenSerialiser, ITokenValidator tokenValidator)
        {
            this._tokenSerialiser = tokenSerialiser;
            this._tokenValidator = tokenValidator;
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
    }
}