using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Federation.Protocols
{
    public class BindingContext
    {
        public BindingContext(IDictionary<string, object> relayState, Uri destinationUri)
        {
            this.ClauseBuilder = new StringBuilder();
            this.DestinationUri = destinationUri;
            this.RelayState = relayState;
        }
        public Uri DestinationUri { get; }
        public StringBuilder ClauseBuilder { get; }
        public IDictionary<string, object> RelayState { get; }
        public virtual Uri GetDestinationUrl()
        {
            return this.DestinationUri;
        }
    }
}