using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.DataProtection;
using Kernel.Extensions;
using Kernel.Federation.Protocols;
using Serialisation.Xml;

namespace Federation.Protocols.Request
{
    public class AuthnRequestBuilder : IAuthnRequestBuilder
    {
        private ICertificateManager _certificateManager;

        public AuthnRequestBuilder(ICertificateManager certificateManager)
        {
            this._certificateManager = certificateManager;
        }

        public Uri BuildRedirectUri(AuthnRequestContext authnRequestContext)
        {
            var configuration = authnRequestContext.Configuration;
            var authnRequest = new AuthnRequest
            {
                Id = "Imperial.flowz.co.uk",
                IsPassive = false,
                Destination = authnRequestContext.Destination.AbsoluteUri,
                Version = "2.0",
                IssueInstant = DateTime.UtcNow
            };
            authnRequest.Issuer = new NameId { Value = "Imperial.flowz.co.uk" };
            var audienceRestrictions = new List<ConditionAbstract>();
            var audienceRestriction = new AudienceRestriction { Audience = new List<string>() { "Imperial.flowz.co.uk" } };
            audienceRestrictions.Add(audienceRestriction);

            authnRequest.Conditions = new Conditions { Items = audienceRestrictions };
            
            var serialiser = new XMLSerialiser();
            
            serialiser.XmlNamespaces.Add("samlp", Saml20Constants.Protocol);
            serialiser.XmlNamespaces.Add("saml", Saml20Constants.Assertion);
            var sb = new StringBuilder();
            using (var ms = new MemoryStream())
            {
                serialiser.Serialize(ms, new[] { authnRequest });
                ms.Position = 0;
                var streamReader = new StreamReader(ms);
                var xmlString = streamReader.ReadToEnd();
                ms.Position = 0;
                var encoded = this.DeflateEncode(xmlString);
                var encodedEscaped = Uri.EscapeDataString(this.UpperCaseUrlEncode(encoded));
                sb.Append("SAMLRequest=");
                sb.Append(encodedEscaped);
                this.SignRequest(sb);
                var result = authnRequest.Destination + "?" + sb.ToString();
                return new Uri(result);
            }
        }

        private void SignRequest(StringBuilder sb)
        {
            //ToDo:
            sb.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.SigAlg, Uri.EscapeDataString(SignedXml.XmlDsigRSASHA1Url));
            this.SignData(sb);
        }
        private StringBuilder SignData(StringBuilder sb)
        {
            //Todo: use configuration
            var cert = this._certificateManager.GetCertificate(@"D:\Dan\Software\Apira\Certificates\TestCertificates\ApiraTestCert.pfx", StringExtensions.ToSecureString("Password1"));
            var dataToSign = Encoding.UTF8.GetBytes(sb.ToString());
           
            var signed = RSADataProtection.SignDataSHA1((RSA)cert.PrivateKey, dataToSign);
            
            var base64 = Convert.ToBase64String(signed);
            var escaped = Uri.EscapeDataString(this.UpperCaseUrlEncode(base64));
            sb.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.Signature, escaped);
            return sb;
        }

        private string UpperCaseUrlEncode(string value)
        {
            var result = new StringBuilder(value);
            for (var i = 0; i < result.Length; i++)
            {
                if (result[i] == '%')
                {
                    result[++i] = char.ToUpper(result[i]);
                    result[++i] = char.ToUpper(result[i]);
                }
            }

            return result.ToString();
        }

        private string DeflateEncode(string val)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(new DeflateStream(memoryStream, CompressionMode.Compress, true), new UTF8Encoding(false)))
                {
                    writer.Write(val);
                    writer.Close();

                    return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, Base64FormattingOptions.None);
                }
            }
        }
    }
}