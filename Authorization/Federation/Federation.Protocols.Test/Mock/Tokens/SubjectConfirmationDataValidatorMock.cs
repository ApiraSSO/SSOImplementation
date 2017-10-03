using System;
using System.IdentityModel.Tokens;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Test.Mock.Tokens
{
    internal class SubjectConfirmationDataValidatorMock : ITokenClauseValidator<Saml2SubjectConfirmationData>
    {
        private Action _action;

        public SubjectConfirmationDataValidatorMock(Action action)
        {
            this._action = action;
        }
        public void Validate(Saml2SubjectConfirmationData clause)
        {
            this._action();
        }
    }
}