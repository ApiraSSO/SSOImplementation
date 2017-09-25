using System;
using System.Runtime.Serialization;
using Kernel.Exceptions;

namespace Kernel.Federation.Exceptions
{
    public class FederationException : BaseException
    {
        public FederationException(string message)
            : base(message)
        {
        }

        public FederationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FederationException() 
        { 
        }

        protected FederationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}