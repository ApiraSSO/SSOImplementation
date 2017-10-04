using System.Xml;

namespace Kernel.Federation.Tokens
{
    public class HandleTokenContext
    {
        public XmlReader Token { get; }
        public object RelayState { get; }
        public string AuthenticationMethod { get; }
        public HandleTokenContext(XmlReader token, object relayState, string authenticationMethod)
        {
            this.Token = token;
            this.RelayState = relayState;
            this.AuthenticationMethod = authenticationMethod;
        }
    }
}