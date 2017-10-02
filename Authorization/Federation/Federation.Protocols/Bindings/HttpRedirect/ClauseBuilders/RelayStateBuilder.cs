using System;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;

namespace Federation.Protocols.Bindings.HttpRedirect.ClauseBuilders
{
    internal class RelayStateBuilder : ISamlClauseBuilder
    {
        private readonly IMessageEncoding _messageEncoding;
        public RelayStateBuilder(IMessageEncoding messageEncoding)
        {
            this._messageEncoding = messageEncoding;
        }
        public uint Order { get { return 1; } }

        public async Task Build(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (String.IsNullOrWhiteSpace(context.RelayState))
                return;

            context.ClauseBuilder.Append("&RelayState=");
            var rsEncoded = await this._messageEncoding.EncodeMessage<string>(context.RelayState);
            var rsEncodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(rsEncoded));
            context.ClauseBuilder.Append(rsEncodedEscaped);
        }
    }
}