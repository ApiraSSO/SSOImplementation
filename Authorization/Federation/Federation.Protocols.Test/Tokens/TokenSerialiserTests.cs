using System.Xml;
using Federation.Protocols.Test.Mock;
using Federation.Protocols.Test.Mock.Tokens;
using Federation.Protocols.Tokens;
using NUnit.Framework;

namespace Federation.Protocols.Test.Tokens
{
    [TestFixture]
    internal class TokenSerialiserTests
    {
        [Test]
        public void DeserialiseTokenTest_Encrypted_assertion()
        {
            //ARRANGE
            var certValidator = new CertificateValidatorMock();
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var xmlReader = XmlReader.Create(@"D:\Dan\Software\Apira\Assertions\20171041624.xml");
            var reader = XmlReader.Create(xmlReader, xmlReader.Settings);
            var tokenHandlerConfigurationProvider = new TokenHandlerConfigurationProvider(federationPartyContextBuilder, certValidator);
            
            var tokenSerialiser = new TokenSerialiser(tokenHandlerConfigurationProvider);
            
            //ACT
            var token = tokenSerialiser.DeserialiseToken(reader, "testshib");
            
            //Assert
            Assert.NotNull(token);
        }

        [Test]
        public void DeserialiseTokenTest_signed_only_assertion()
        {
            //ARRANGE
            var path = @"D:\Dan\Software\Apira\Assertions\FromLocal\20171041056.xml";
            var certValidator = new CertificateValidatorMock();
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var xmlReader = XmlReader.Create(path);
            var reader = XmlReader.Create(xmlReader, xmlReader.Settings);
            var tokenHandlerConfigurationProvider = new TokenHandlerConfigurationProvider(federationPartyContextBuilder, certValidator);

            var tokenSerialiser = new TokenSerialiser(tokenHandlerConfigurationProvider);
           
            //ACT
            var token = tokenSerialiser.DeserialiseToken(reader, "testshib");

            //Assert
            Assert.NotNull(token);
        }

        [Test]
        public void DeserialiseTokenTest_signed_only_assertion_raw()
        {
            //ARRANGE
            var path = @"D:\Dan\Software\Apira\Assertions\FromLocal\20171041550.xml";
            var certValidator = new CertificateValidatorMock();
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var xmlReader = XmlReader.Create(path);
            var reader = XmlReader.Create(xmlReader, xmlReader.Settings);
            var tokenHandlerConfigurationProvider = new TokenHandlerConfigurationProvider(federationPartyContextBuilder, certValidator);
            var configuration = tokenHandlerConfigurationProvider.GetConfiguration("testshib");
            var saml2SecurityTokenHandler = new SecurityTokenHandlerMock();
            saml2SecurityTokenHandler.SetConfiguration(configuration);
            //ACT
            var assertion = saml2SecurityTokenHandler.GetAssertion(reader);

            //Assert
            Assert.NotNull(assertion);
        }
    }
}