using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface IRelayStateHandler
    {
        Task<object> GetRelayStateFromFormData(IDictionary<string, string> form);
    }
}