using System.IdentityModel.Tokens;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens.Validation
{
    internal class SubjectConfirmationDataValidator : ITokenClauseValidator<Saml2SubjectConfirmationData>
    {
        public void Validate(Saml2SubjectConfirmationData clause)
        {
            //do nothing for now
        }
    }
}