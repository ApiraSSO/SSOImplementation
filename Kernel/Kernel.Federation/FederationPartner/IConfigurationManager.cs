using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Federation.FederationPartner
{
    public interface IConfigurationManager<T> where T : class
    {
        Task<T> GetConfigurationAsync(string federationPartyId, CancellationToken cancel);

        void RequestRefresh(string federationPartyId);
    }
}