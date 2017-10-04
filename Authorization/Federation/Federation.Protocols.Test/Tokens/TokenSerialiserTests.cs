using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Federation.Protocols.Request;
using Federation.Protocols.Request.Elements;
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

        [Test]
        public void DeserialiseTokenTest_signed_only_assertion_raw1()
        {
            //ARRANGE
            var path = @"D:\Dan\Software\Apira\Assertions\20171041824.xml";
            var doc = new XmlDocument();
            doc.Load(path);
            var el = doc.DocumentElement;
            var inner = new X509CertificateStoreTokenResolver("TestCertStore", StoreLocation.LocalMachine);

            var encryptedList = el.GetElementsByTagName(EncryptedAssertion.ElementName, Saml20Constants.Assertion);
            XmlDocument result = null;

            //ACT
            if (encryptedList.Count == 1)
            {
                var encryptedAssertion = (XmlElement)encryptedList[0];

                result = TokenHelper.GetPlainAsertion(inner, encryptedAssertion);
            }

            var valid = TokenHelper.VerifySignature(result.DocumentElement);
            Assert.IsTrue(valid);
        }

        [Test]
        public void DeserialiseTokenTest_signed_only_assertion_raw2()
        {
            //ARRANGE
            var path = @"D:\Dan\Software\Apira\Assertions\FromLocal\20171041550.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(path);
            var valid = TokenHelper.VerifySignature(xmlDoc.DocumentElement);
           
            Assert.IsTrue(valid);
        }
    }
}