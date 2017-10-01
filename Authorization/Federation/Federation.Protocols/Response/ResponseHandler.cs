using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Federation.Protocols.Request;
using Kernel.Compression;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    internal class ResponseHandler : IReponseHandler
    {
        private readonly ICompression _compression;
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        private readonly ITokenHandler _tokenHandler;
        public ResponseHandler(ICompression compression, IFederationPartyContextBuilder federationPartyContextBuilder, ITokenHandler tokenHandler)
        {
            this._compression = compression;
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._tokenHandler = tokenHandler;
        }
        public async Task Handle(Func<IDictionary<string, string>> parser)
        {
            var elements = parser();
            var responseBase64 = elements["SAMLResponse"];
            var responseBytes = Convert.FromBase64String(responseBase64);
            var responseText = Encoding.UTF8.GetString(responseBytes);
            
            //this.SaveTemp(responseText);
            var xmlReader = XmlReader.Create(new StringReader(responseText));
            this.ValidateResponseSuccess(xmlReader);
            var assertion = this._tokenHandler.GetAssertion(xmlReader);

            var relayStateCompressed = elements["RelayState"];
            var decompressed = await Helper.DeflateDecompress(relayStateCompressed, this._compression);
            throw new NotImplementedException();
        }

        private void ValidateResponseSuccess(XmlReader reader)
        {
            while (!reader.IsStartElement("StatusCode", "urn:oasis:names:tc:SAML:2.0:protocol"))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find status code element.");
            }
            var status = reader.GetAttribute("Value");
            if (String.IsNullOrWhiteSpace(status) || !String.Equals(status, "urn:oasis:names:tc:SAML:2.0:status:Success"))
                throw new Exception(status);
        }

        private void SaveTemp(string responseText)
        {
            var writer = XmlWriter.Create(@"D:\Dan\Software\Apira\a.xml");
            var el = new XmlDocument();
            el.Load(new StringReader(responseText));
            el.DocumentElement.WriteTo(writer);
            writer.Flush();
            writer.Dispose();
        }
    }
}