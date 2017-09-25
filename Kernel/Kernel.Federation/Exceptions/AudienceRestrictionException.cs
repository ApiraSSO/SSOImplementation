using System;

namespace Kernel.Federation.Exceptions
{
    public class AudienceRestrictionException : FederationException
    {
        public override string Message
        {
            get
            {
                return "AudienceRestriction has been violated. See the inner exception for details.";
            }
        }

        public AudienceRestrictionException(Exception ex)
            : base(null, ex)
        { }
    }
}
