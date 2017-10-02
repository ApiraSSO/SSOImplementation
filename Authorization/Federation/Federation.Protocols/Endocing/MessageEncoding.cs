using System;
using System.Threading.Tasks;
using Kernel.Compression;
using Kernel.Federation.Protocols;

namespace Federation.Protocols.Endocing
{
    internal class MessageEncoding : IMessageEncoding
    {
        private readonly ICompression _compression;
        public MessageEncoding(ICompression compression)
        {
            this._compression = compression;
        }

        public Task<TMessage> DecodeMessage<TMessage>(string message)
        {
            throw new NotImplementedException();
        }

        public async Task<string> DecodeMessage(string message)
        {
            return await Helper.DeflateDecompress(message, this._compression);
        }

        public async Task<string> EncodeMessage<TMessage>(TMessage message)
        {
            return await Helper.DeflateEncode(message.ToString(), this._compression);
        }
    }
}