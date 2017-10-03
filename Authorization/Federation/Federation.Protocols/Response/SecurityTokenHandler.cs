using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Federation.Protocols.Request;
using Federation.Protocols.Request.Elements;
using Kernel.Cryptography.Validation;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    public class SecurityTokenHandler : Saml2SecurityTokenHandler, ITokenHandler, ITokenValidator
    {
        ITokenHandlerConfigurationProvider _tokenHandlerConfigurationProvider;

        public SecurityTokenHandler(ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider)
        {
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
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
            //base.ValidateConfirmationData(confirmationData);
        }
        
        internal void SetConfigurationFor(string partnerId)
        {
            this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
        }
    }
}