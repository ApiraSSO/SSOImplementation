using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Xml;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens
{
    internal class TokenSerialiser : ITokenSerialiser
    {
        private readonly ITokenConfigurationProvider<SecurityTokenHandlerConfiguration> _tokenHandlerConfigurationProvider;

        private readonly Saml2SecurityTokenHandler _saml2SecurityTokenHandler = new Saml2SecurityTokenHandler();
        public TokenSerialiser(ITokenConfigurationProvider<SecurityTokenHandlerConfiguration> tokenHandlerConfigurationProvider)
        {
            this._tokenHandlerConfigurationProvider = tokenHandlerConfigurationProvider;
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

        public SecurityToken DeserialiseToken(XmlReader reader, string partnerId)
        {
            var configuration = this._tokenHandlerConfigurationProvider.GetConfiguration(partnerId);
            this._saml2SecurityTokenHandler.Configuration = configuration;
            TokenHelper.MoveToToken(reader);
            return this._saml2SecurityTokenHandler.ReadToken(reader);
        }

        public void Serialize(Stream stream, object[] o)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object o)
        {
            throw new NotImplementedException();
        }
    }
}