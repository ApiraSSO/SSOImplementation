using System;
using System.Text;

namespace Kernel.Federation.Protocols
{
    public class BindingContext
    {
        public BindingContext(string relayState, Uri destinationUri)
        {
            this.ClauseBuilder = new StringBuilder();
            this.DestinationUri = destinationUri;
            this.RelayState = relayState;
        }
        public Uri DestinationUri { get; }
        public StringBuilder ClauseBuilder { get; }
        public string RelayState { get; }
        public virtual Uri GetDestinationUrl()
        {
            return this.DestinationUri;
        }
    }
}