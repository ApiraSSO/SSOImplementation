using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;
using Serialisation.Xml;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Request
{
    internal class AuthnRequestSerialiser : IAuthnRequestSerialiser
    {
        private readonly IXmlSerialiser _serialiser;
        private readonly IMessageEncoding _messageEncoding;
        public AuthnRequestSerialiser(IXmlSerialiser serialiser, IMessageEncoding messageEncoding)
        {
            this._serialiser = serialiser;
            this._messageEncoding = messageEncoding;
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

        public void Serialize(Stream stream, object[] o)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object o)
        {
            this._serialiser.XmlNamespaces.Add("samlp", Saml20Constants.Protocol);
            this._serialiser.XmlNamespaces.Add("saml", Saml20Constants.Assertion);

            using (var ms = new MemoryStream())
            {
                this._serialiser.Serialize(ms, new[] { o });
                ms.Position = 0;
                var streamReader = new StreamReader(ms);
                var xmlString = streamReader.ReadToEnd();
                var compressed = Task.Factory.StartNew<Task<string>>(async() => await this._messageEncoding.EncodeMessage<string>(xmlString));
                compressed.Wait();
                var encodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(compressed.Result.Result));
                return encodedEscaped;
            }
        }
    }
}