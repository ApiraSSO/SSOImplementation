using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Federation.Protocols.Test.Mock;
using Federation.Protocols.Test.Mock.Tokens;
using Federation.Protocols.Tokens;
using NUnit.Framework;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Test.Tokens
{
    [TestFixture]
    internal class TokenSerialiserTests
    {
        [Test]
        public void DeserialiseTokenTest_Encrypted_assertion()
        {
            //ARRANGE
            var dirPath = @"D:\Dan\Software\Apira\Assertions\";
            var path = FileHelper.GetLastesFile(dirPath);
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
        public void DeserialiseTokenTest_signed_only_assertion()
        {
            //ARRANGE
            
            var dirPath = @"D:\Dan\Software\Apira\Assertions\Local\";
            var path = FileHelper.GetLastesFile(dirPath);
            var certValidator = new CertificateValidatorMock();
            var federationPartyContextBuilder = new FederationPartyContextBuilderMock();
            var xmlReader = XmlReader.Create(path);
            var reader = XmlReader.Create(xmlReader, xmlReader.Settings);
            var tokenHandlerConfigurationProvider = new TokenHandlerConfigurationProvider(federationPartyContextBuilder, certValidator);

            var tokenSerialiser = new TokenSerialiser(tokenHandlerConfigurationProvider);
           
            //ACT
            var token = tokenSerialiser.DeserialiseToken(reader, "local");

            //Assert
            Assert.NotNull(token);
        }

        [Test]
        public void DeserialiseTokenTest_signed_only_assertion_read_assertion()
        {
            //ARRANGE
            
            var dirPath = @"D:\Dan\Software\Apira\Assertions\Local\";
            var path = FileHelper.GetLastesFile(dirPath);
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
        public void DeserialiseTokenTest_encrypted_assertion_manual_signarure_verification()
        {
            //ARRANGE
            
            var dirPath = @"D:\Dan\Software\Apira\Assertions\";
            var path = FileHelper.GetLastesFile(dirPath);
            var doc = new XmlDocument();
            doc.Load(path);
            var el = doc.DocumentElement;
            var inner = new X509CertificateStoreTokenResolver("TestCertStore", StoreLocation.LocalMachine);

            var encryptedList = el.GetElementsByTagName(HttpRedirectBindingConstants.EncryptedAssertion, Saml20Constants.Assertion);
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
        [Ignore("Access local file. To be used only as a cross reference.")]
        public void DeserialiseTokenTest_signed_only_assertion_manual_signature_verification()
        {
            //ARRANGE
            
            var dirPath = @"D:\Dan\Software\Apira\Assertions\Local\";
            var path = FileHelper.GetLastesFile(dirPath);
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(path);
            var valid = TokenHelper.VerifySignature(xmlDoc.DocumentElement);
           
            Assert.IsTrue(valid);
        }
    }
}