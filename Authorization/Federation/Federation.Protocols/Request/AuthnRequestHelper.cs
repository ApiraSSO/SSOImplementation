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
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

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
            
            await AuthnRequestHelper.AppendRequest(sb, xmlString, compression);
            
            if(!String.IsNullOrWhiteSpace(authnRequestContext.RelyingState))
            {
                await AuthnRequestHelper.AppendRelyingState(sb, authnRequestContext, compression);
            }

            if (spDescriptor.AuthenticationRequestsSigned)
            {
                AuthnRequestHelper.SignRequest(sb, kd, certificateManager);
            }
            return sb.ToString();
        }

        internal static string Serialise(AuthnRequest request, IXmlSerialiser serialiser)
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

        internal static async Task AppendRequest(StringBuilder builder, string request, ICompression compression)
        {
            var compressed = await Helper.DeflateEncode(request, compression);
            var encodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(compressed));
            builder.Append("SAMLRequest=");
            builder.Append(encodedEscaped);
        }
        internal static async Task AppendRelyingState(StringBuilder builder, AuthnRequestContext authnRequestContext, ICompression compression)
        {
            builder.Append("&RelayState=");
            var rsEncoded = await Helper.DeflateEncode(authnRequestContext.RelyingState, compression);
            var rsEncodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(rsEncoded));
            builder.Append(rsEncodedEscaped);
        }

        internal static void AppendSignarureAlgorithm(StringBuilder builder)
        {
            builder.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.SigAlg, Uri.EscapeDataString(SignedXml.XmlDsigRSASHA1Url));
        }
        internal static void SignRequest(StringBuilder sb, CertificateContext certContext, ICertificateManager manager)
        {
            AuthnRequestHelper.AppendSignarureAlgorithm(sb);
            AuthnRequestHelper.SignData(sb, certContext, manager);
        }
        internal static StringBuilder SignData(StringBuilder sb, CertificateContext certContext, ICertificateManager manager)
        {
            var base64 = manager.SignToBase64(sb.ToString(), certContext);
            var escaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(base64));
            sb.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.Signature, escaped);
            return sb;
        }

        private static IEnumerable<IAuthnRequestClauseBuilder<AuthnRequest>> GetBuilders()
        {
            return ReflectionHelper.GetAllTypes(new[] { typeof(ClauseBuilder).Assembly }, t => AuthnRequestHelper._condition(t))
                .Select(x => (IAuthnRequestClauseBuilder<AuthnRequest>)Activator.CreateInstance(x));
        }
    }
}