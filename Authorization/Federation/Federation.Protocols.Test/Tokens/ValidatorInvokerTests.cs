using System.IdentityModel.Tokens;
using Federation.Protocols.Test.Mock.Tokens;
using Federation.Protocols.Tokens.Validation;
using NUnit.Framework;

namespace Federation.Protocols.Test.Tokens
{
    [TestFixture]
    internal class ValidatorInvokerTests
    {
        [Test]
        public void InvokeClausValidatorTest()
        {
            //ARRANGE
            var result = false;
            var validator = new SubjectConfirmationDataValidatorMock(() => result = true);
            var invoker = new ValidatorInvoker(t => validator);
            var arg = new Saml2SubjectConfirmationData();
            //ACT
            invoker.Validate(arg);
            //ASSERT
            Assert.IsTrue(result);
        }
    }
}
