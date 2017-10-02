using System.Text;

namespace Kernel.Federation.Protocols
{
    public class BindingContext
    {
        public BindingContext(string relayState)
        {
            this.ClauseBuilder = new StringBuilder();
            this.RelayState = relayState;
        }
        public StringBuilder ClauseBuilder { get; }
        public string RelayState { get; }
    }
}