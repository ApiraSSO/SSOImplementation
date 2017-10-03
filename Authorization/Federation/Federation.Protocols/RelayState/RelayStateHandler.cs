using System.Collections.Generic;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;

namespace Federation.Protocols.RelayState
{
    internal class RelayStateHandler : IRelayStateHandler
    {
        private readonly IRelayStateSerialiser _relayStateSerialiser;
        public RelayStateHandler(IRelayStateSerialiser relayStateSerialiser)
        {
            this._relayStateSerialiser = relayStateSerialiser;
        }
        public async Task<object> GetRelayStateFromFormData(IDictionary<string, string> form)
        {
            var relayStateCompressed = form["RelayState"];
            var relayState = await this._relayStateSerialiser.Deserialize(relayStateCompressed);
            return relayState;
        }
    }
}