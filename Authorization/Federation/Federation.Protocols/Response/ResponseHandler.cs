using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var relayStateCompressed = elements["RelayState"];
            var decompressed = await Helper.DeflateDecompress(relayStateCompressed, this._compression);
            throw new NotImplementedException();
        }
    }
}