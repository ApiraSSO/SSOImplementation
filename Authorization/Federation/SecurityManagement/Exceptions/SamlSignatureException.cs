using System;
using System.Runtime.Serialization;
using Kernel.Exceptions;

namespace SecurityManagement.Exceptions
{
    public class SamlSignatureException : BaseException
    {
        public SamlSignatureException()
        {
        }

        public SamlSignatureException(string message) : base(message)
        {
        }

        public SamlSignatureException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SamlSignatureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}