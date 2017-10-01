using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Kernel.Compression;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    internal class ResponseHandler : IReponseHandler
    {
        private readonly ICompression _compression;
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        public ResponseHandler(ICompression compression, IFederationPartyContextBuilder federationPartyContextBuilder)
        {
            this._compression = compression;
            this._federationPartyContextBuilder = federationPartyContextBuilder;
        }
        public async Task Handle(Func<IDictionary<string, string>> parser)
        {
            var elements = parser();
            var responseBase64 = elements["SAMLResponse"];
            var responseBytes = Convert.FromBase64String(responseBase64);
            var responseText = Encoding.UTF8.GetString(responseBytes);
            var h = new Saml2SecurityTokenHandler();
            //this.SaveTemp(responseText);
            var xmlReader = XmlReader.Create(new StringReader(responseText));
            
            var b = h.CanReadToken(xmlReader);
            var relayStateCompressed = elements["RelayState"];
            var decompressed = await Helper.DeflateDecompress(relayStateCompressed, this._compression);
            throw new NotImplementedException();
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