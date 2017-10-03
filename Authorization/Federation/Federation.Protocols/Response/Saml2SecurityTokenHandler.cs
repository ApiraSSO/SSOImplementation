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
    public class Saml2SecurityTokenHandler : System.IdentityModel.Tokens.Saml2SecurityTokenHandler, ITokenHandler, ITokenValidator
    {
        ITokenHandlerConfigurationProvider _tokenHandlerConfigurationProvider;

        public Saml2SecurityTokenHandler(ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider)
        {
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
        }
        public IEnumerable<ClaimsIdentity> Claims { get; private set; }
        public Saml2Assertion GetAssertion(XmlReader reader, string partnerId)
        {
            this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
            this.MoveToToken(reader);

            return this.ReadAssertion(reader);
        }

        public  SecurityToken ReadToken(XmlReader reader, string partnerId)
        {
            this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
            this.MoveToToken(reader);
            return base.ReadToken(reader);
        }

        public bool Validate(SecurityToken token, ICollection<ValidationResult> validationResult, string partnerId)
        {
            try
            {
                ((ICertificateValidator)base.CertificateValidator).SetFederationPartyId(partnerId);
                this.Claims = base.ValidateToken(token);
                return true;
            }catch(Exception ex)
            {
                validationResult.Add(new ValidationResult(ex.Message));
                return false;
            }
        }
        protected override void ValidateConfirmationData(Saml2SubjectConfirmationData confirmationData)
        {
            //base.ValidateConfirmationData(confirmationData);
        }
        internal XmlDocument GetPlainTestAsertion(XmlElement el, string partnerId)
        {
            this._tokenHandlerConfigurationProvider.Configuration(this, partnerId);
            var encryptedDataElement = GetElement(Federation.Protocols.Request.Elements.Xenc.EncryptedData.ElementName, Saml20Constants.Xenc, el);

            var encryptedData = new System.Security.Cryptography.Xml.EncryptedData();
            encryptedData.LoadXml(encryptedDataElement);
            var encryptedKey = new System.Security.Cryptography.Xml.EncryptedKey();
            var encryptedKeyElement = GetElement(Federation.Protocols.Request.Elements.Xenc.EncryptedKey.ElementName, Saml20Constants.Xenc, el);

            encryptedKey.LoadXml(encryptedKeyElement);
            var securityKeyIdentifier = new SecurityKeyIdentifier();
            foreach (KeyInfoX509Data v in encryptedKey.KeyInfo)
            {
                foreach (X509Certificate2 cert in v.Certificates)
                {
                    var cl = new X509RawDataKeyIdentifierClause(cert);
                    securityKeyIdentifier.Add(cl);
                }
            }

            var clause = new EncryptedKeyIdentifierClause(encryptedKey.CipherData.CipherValue, encryptedKey.EncryptionMethod.KeyAlgorithm, securityKeyIdentifier);
            SecurityKey key;
            var success = base.Configuration.ServiceTokenResolver.TryResolveSecurityKey(clause, out key);
            if (!success)
                throw new InvalidOperationException("Cannot locate security key");

            SymmetricSecurityKey symmetricSecurityKey = key as SymmetricSecurityKey;
            if (symmetricSecurityKey == null)
                throw new InvalidOperationException("Key must be symmentric key");

            SymmetricAlgorithm symmetricAlgorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptedData.EncryptionMethod.KeyAlgorithm);
            var encryptedXml = new System.Security.Cryptography.Xml.EncryptedXml();
            
            var plaintext = encryptedXml.DecryptData(encryptedData, symmetricAlgorithm);
            var assertion = new XmlDocument { PreserveWhitespace = true };

            assertion.Load(new StringReader(Encoding.UTF8.GetString(plaintext)));
            return assertion;
        }

        private void MoveToToken(XmlReader reader)
        {
            while (!reader.IsStartElement(EncryptedAssertion.ElementName, Saml20Constants.Assertion))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find assertion element.");
            }
        }

        private static XmlElement GetElement(string element, string elementNS, XmlElement doc)
        {
            var list = doc.GetElementsByTagName(element, elementNS);
            return list.Count == 0 ? null : (XmlElement)list[0];
        }
        
    }
}