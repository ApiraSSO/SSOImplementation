using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Federation.Protocols.Request.ClauseBuilders;
using Kernel.Compression;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols;
using Kernel.Reflection;
using Serialisation.Xml;

namespace Federation.Protocols.Request
{
    internal class AuthnRequestHelper
    {
        private static Func<Type, bool> _condition = t => !t.IsAbstract && !t.IsInterface && typeof(IAuthnRequestClauseBuilder<AuthnRequest>).IsAssignableFrom(t);
        internal static AuthnRequest BuildAuthnRequest(AuthnRequestContext authnRequestContext, IFederationPartyContextBuilder federationPartyContextBuilder)
        {
            var federationParty = federationPartyContextBuilder.BuildContext(authnRequestContext.FederationPartyId);
            var request = new AuthnRequest
            {
                IsPassive = false,
                ForceAuthn = false,
                Destination = authnRequestContext.Destination.AbsoluteUri,
                Version = authnRequestContext.Version,
                IssueInstant = DateTime.UtcNow
            };
            var buiders = AuthnRequestHelper.GetBuilders();
            foreach(var b in buiders)
            {
                b.Build(request, federationParty);
            }
            return request;
        }

        internal static async Task<string> SerialiseAndSign(AuthnRequest request, AuthnRequestContext authnRequestContext, IXmlSerialiser serialiser, IFederationPartyContextBuilder federationPartyContextBuilder, ICertificateManager certificateManager, ICompression compression)
        {
            var federationParty = federationPartyContextBuilder.BuildContext(authnRequestContext.FederationPartyId);
            var metadataContext = federationParty.MetadataContext;
            var entityDescriptor = metadataContext.EntityDesriptorConfiguration;
            var spDescriptor = entityDescriptor.SPSSODescriptors
                .First();
            var kd = spDescriptor.KeyDescriptors.First(x => x.IsDefault && x.Use == Kernel.Federation.MetaData.Configuration.Cryptography.KeyUsage.Signing)
                .CertificateContext;
            var sb = new StringBuilder();
            var xmlString = AuthnRequestHelper.Serialise(request, serialiser);
            var encoded = await Helper.DeflateEncode(xmlString, compression);
            var encodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(encoded));
            sb.Append("SAMLRequest=");
            sb.Append(encodedEscaped);
            if(!String.IsNullOrWhiteSpace(authnRequestContext.RelyingState))
            {
                sb.Append("&RelayState=");
                var rsEncoded = await Helper.DeflateEncode(authnRequestContext.RelyingState, compression);
                var rsEncodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(rsEncoded));
                sb.Append(rsEncodedEscaped);
            }
            if (spDescriptor.AuthenticationRequestsSigned)
            {
                AuthnRequestHelper.SignRequest(sb, kd, certificateManager);
            }
            return sb.ToString();
        }

        public static string Serialise(AuthnRequest request, IXmlSerialiser serialiser)
        {
            serialiser.XmlNamespaces.Add("samlp", Saml20Constants.Protocol);
            serialiser.XmlNamespaces.Add("saml", Saml20Constants.Assertion);
            
            using (var ms = new MemoryStream())
            {
                serialiser.Serialize(ms, new[] { request });
                ms.Position = 0;
                var streamReader = new StreamReader(ms);
                var xmlString = streamReader.ReadToEnd();
                return xmlString;
            }
        }
        private static IEnumerable<IAuthnRequestClauseBuilder<AuthnRequest>> GetBuilders()
        {
            return ReflectionHelper.GetAllTypes(new[] { typeof(ClauseBuilder).Assembly }, t => AuthnRequestHelper._condition(t))
                .Select(x => (IAuthnRequestClauseBuilder<AuthnRequest>)Activator.CreateInstance(x));
        }

        private static void SignRequest(StringBuilder sb, CertificateContext certContext, ICertificateManager manager)
        {
            //ToDo:
            sb.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.SigAlg, Uri.EscapeDataString(SignedXml.XmlDsigRSASHA1Url));
            AuthnRequestHelper.SignData(sb, certContext, manager);
        }
        private static StringBuilder SignData(StringBuilder sb, CertificateContext certContext, ICertificateManager manager)
        {
            var base64 = manager.SignToBase64(sb.ToString(), certContext);
            var escaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(base64));
            sb.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.Signature, escaped);
            return sb;
        }
    }
}