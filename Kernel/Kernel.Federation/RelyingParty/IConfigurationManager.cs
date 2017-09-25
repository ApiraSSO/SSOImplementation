using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Federation.RelyingParty
{
    public interface IConfigurationManager<T> where T : class
    {
        Task<T> GetConfigurationAsync(string relyingPartyId, CancellationToken cancel);

        void RequestRefresh(string relyingPartyId);
    }
}