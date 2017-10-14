using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;
using Kernel.Logging;
using Kernel.Serialisation;
using Serialisation.Xml;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Request
{
    internal class AuthnRequestSerialiser : IAuthnRequestSerialiser
    {
        private readonly IXmlSerialiser _serialiser;
        private readonly IMessageEncoding _messageEncoding;
        private readonly ILogProvider _logProvider;

        public AuthnRequestSerialiser(IXmlSerialiser serialiser, IMessageEncoding messageEncoding, ILogProvider logProvider)
        {
            this._serialiser = serialiser;
            this._messageEncoding = messageEncoding;
            this._logProvider = logProvider;
        }

        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string data)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(string data)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Serialize(object o)
        {
            this._serialiser.XmlNamespaces.Add("samlp", Saml20Constants.Protocol);
            this._serialiser.XmlNamespaces.Add("saml", Saml20Constants.Assertion);

            using (var ms = new MemoryStream())
            {
                this._serialiser.Serialize(ms, new[] { o });
                ms.Position = 0;
                var streamReader = new StreamReader(ms);
                var xmlString = streamReader.ReadToEnd();
                this._logProvider.LogMessage(String.Format("AuthnRequest serialised:\r\n {0}", xmlString));
                var compressed = await this._messageEncoding.EncodeMessage<string>(xmlString);
                var encodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(compressed));
                return encodedEscaped;
            }
        }

        void ISerializer.Serialize(Stream stream, object[] o)
        {
            throw new NotImplementedException();
        }

        string ISerializer.Serialize(object o)
        {
            throw new NotImplementedException();
        }
    }
}