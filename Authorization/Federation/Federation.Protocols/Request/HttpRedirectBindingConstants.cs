using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Federation.Protocols.Request
{
    internal class HttpRedirectBindingConstants
    {
        /// <summary>
        /// SAMLResponse name
        /// </summary>
        public const string SamlResponse = "SAMLResponse";

        /// <summary>
        /// SAMLRequest name
        /// </summary>
        public const string SamlRequest = "SAMLRequest";

        /// <summary>
        /// Signature Algorithm name
        /// </summary>
        public const string SigAlg = "SigAlg";

        /// <summary>
        /// Relay state name
        /// </summary>
        public const string RelayState = "RelayState";

        /// <summary>
        /// Signature name
        /// </summary>
        public const string Signature = "Signature";
    }
}
